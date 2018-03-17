using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Expression.Interactivity.Core;

namespace BeeHive
{
	public class CollisionInformation
	{
		public Rect CollisionRect
		{
			get;
			private set;
		}

		public FrameworkElement CollidingElement
		{
			get;
			private set;
		}

		public FrameworkElement HitElement
		{
			get;
			private set;
		}

		public Vector Normal
		{
			get;
			private set;
		}

		public CollisionInformation(FrameworkElement collidingElement, FrameworkElement hitElement, Rect collisionRect, Vector normal)
		{
			this.CollidingElement = collidingElement;
			this.HitElement = hitElement;
			this.CollisionRect = collisionRect;
			this.Normal = normal;
		}
	}

	public class GameEnvironment : Behavior<FrameworkElement>
	{
		// Private Properties
		private Random random;
		private Canvas game;
		private Control mainControl;
		private PropertyDictionary propertyDictionary;

		private DispatcherTimer timer;

		private const int NUM_LEVELS = 3;
		private const int MAX_NUM_LEVELS = 6;

		public event EventHandler Reset;
		public event EventHandler LevelChanging;
		public event EventHandler GameOver;
		public event EventHandler ElementRemoved;

		public event EventHandler GameLoop;
		public event EventHandler<CollisionEventArgs> Collision;

		private List<FrameworkElement> collidableElementsToRemove = new List<FrameworkElement>();

		private Dictionary<FrameworkElement, ICollidable> collidableElements = new Dictionary<FrameworkElement, ICollidable>();
		private Dictionary<FrameworkElement, ICollidable> collisionElements = new Dictionary<FrameworkElement, ICollidable>();


		public int Level
		{
			get;
			private set;
		}

		#region Dependency Properties

		public static readonly DependencyProperty GameOverSoundProperty = DependencyProperty.Register("GameOverSound", typeof(System.Uri), typeof(GameEnvironment), null);
		public static readonly DependencyProperty NextLevelSoundProperty = DependencyProperty.Register("NextLevelSound", typeof(System.Uri), typeof(GameEnvironment), null);
		public static readonly DependencyProperty GameOverStateProperty = DependencyProperty.Register("GameOverState", typeof(string), typeof(GameEnvironment), null);

		[Category("Game States Properties")]
		public System.Uri GameOverSound { get { return (System.Uri)GetValue(GameOverSoundProperty); } set { SetValue(GameOverSoundProperty, value); } }
		[Category("Game States Properties")]
		public System.Uri NextLevelSound { get { return (System.Uri)GetValue(NextLevelSoundProperty); } set { SetValue(NextLevelSoundProperty, value); } }
		[Category("Game States Properties")]
		public string GameOverState { get { return (string)GetValue(GameOverStateProperty); } set { SetValue(GameOverStateProperty, value); } }

		#endregion

		protected override void OnAttached()
		{
			base.OnAttached();
			game = this.AssociatedObject as Canvas;
			this.AssociatedObject.Loaded += this.OnAssociatedObjectLoaded;
		}

		public FrameworkElement RootElement
		{
			get { return this.AssociatedObject; }
		}

		public PropertyDictionary PropertyDictionary
		{
			get { return this.propertyDictionary; }
		}

		public ICommand ResetCommand
		{
			get;
			private set;
		}

		public GameEnvironment()
		{
			this.random = new Random();
			this.propertyDictionary = new PropertyDictionary();
			this.ResetCommand = new ActionCommand(this.HandleReset);
			this.PropertyDictionary.SetValue<int>("Score", 0);
			this.PropertyDictionary.SetValue<int>("Lives", 4);
			this.PropertyDictionary.SetValue<int>("HighScore", DesignerProperties.IsInDesignTool ? 0 : this.ReadHighscore());
			this.PropertyDictionary.SetValue<int>("LastScore", 0);
			this.PropertyDictionary.RegisterPropertyChangeHandler("Lives", this.OnLivesChanged);
		}

		private void OnLivesChanged(object oldValue, object newValue)
		{
			if ((int)newValue == 0)
			{
				this.EndGame();
			}
		}

		private void EndGame()
		{
			VisualStateManager.GoToState(this.mainControl, "ShowGameOver", true);
			VisualStateManager.GoToState(this.mainControl, "GameOverLevel", true);

			int score = this.PropertyDictionary.GetValue<int>("Score");
			int highScore = this.PropertyDictionary.GetValue<int>("HighScore");
			this.PropertyDictionary.SetValue<int>("LastScore", score);
			if (score > highScore)
			{
				this.SaveHighscore(score);
				this.PropertyDictionary.SetValue<int>("HighScore", score);
			}
			this.PropertyDictionary.SetValue<int>("Score", 0);
			this.PropertyDictionary.SetValue<int>("Lives", 4);

			if (this.GameOver != null)
			{
				this.GameOver(this, EventArgs.Empty);
			}
		}

		private void HandleReset()
		{
			if (this.Reset != null)
			{
				this.Reset(this, EventArgs.Empty);
			}
		}

		private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
		{
			// Finds the topmost User Control. This reference is used to trigger User Control States
			DependencyObject parent = this.AssociatedObject as DependencyObject;
			while (parent != null)
			{
				mainControl = parent as Control;
				parent = VisualTreeHelper.GetParent(parent) as DependencyObject;
			}

			// Start at Level 1
			this.Level = 1;
			if (mainControl != null)
			{
				VisualStateManager.GoToState(mainControl, "Level1", true);
			}

			// Starts main Game Loop
			this.timer = new DispatcherTimer();
			this.timer.Interval = TimeSpan.FromMilliseconds(15);
			this.timer.Tick += this.OnTimerTick;
			this.timer.Start();
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			List<CollisionInformation> collisions = new List<CollisionInformation>();
			// this list should be relatively small
			foreach (KeyValuePair<FrameworkElement, ICollidable> collisionKeyValuePair in this.collisionElements)
			{
				FrameworkElement collisionElement = collisionKeyValuePair.Key;
				ICollidable collision = collisionKeyValuePair.Value;
				Rect collisionRect = collision.Position;
				// this list may be large
				foreach (KeyValuePair<FrameworkElement, ICollidable> collidedKeyValuePair in this.collidableElements)
				{
					FrameworkElement collidedElement = collidedKeyValuePair.Key;
					ICollidable collidable = collidedKeyValuePair.Value;
					Rect collidableRect = collidable.Position;

					collidableRect.Intersect(collisionRect);
					if (collidableRect != Rect.Empty)
					{
						// a collision has occurred
						if (this.Collision != null)
						{
							Vector collisionVector;
							double perturbX = ((this.random.NextDouble() - 0.5) * 0.05);
							double perturbY = ((this.random.NextDouble() - 0.5) * 0.05);

							if (collidableRect.Width >= collidableRect.Height)
							{
								collisionVector = new Vector(perturbX, -1 + perturbY);
								collisionVector.Normalize();
							}
							else
							{
								collisionVector = new Vector(-1 + perturbX, perturbY);
								collisionVector.Normalize();
							}

							collisions.Add(new CollisionInformation(collisionElement, collidedElement, collidableRect, collisionVector));
						}
					}
				}
			}

			if (collisions.Count > 0 && this.Collision != null)
			{
				this.Collision(this, new CollisionEventArgs(collisions));
			}

			for (int i = 0; i < this.collidableElementsToRemove.Count; i++)
			{
				this.collidableElements.Remove(this.collidableElementsToRemove[i]);

				if (this.ElementRemoved != null)
				{
					this.ElementRemoved(this, EventArgs.Empty);
				}
			}
			this.collidableElementsToRemove.Clear();

			if (this.GameLoop != null)
			{
				this.GameLoop(this, null);
			}	
		}

		public class CollisionEventArgs : EventArgs
		{
			public IEnumerable<CollisionInformation> CollisionInformation
			{
				get;
				private set;
			}

			public CollisionEventArgs(IList<CollisionInformation> collisions)
			{
				this.CollisionInformation = new ReadOnlyCollection<CollisionInformation>(collisions);
			}
		}

		public void RegisterAsCollider(FrameworkElement collider)
		{
			foreach(Behavior behavior in Interaction.GetBehaviors(collider))
			{
				if (behavior is ICollidable)
				{
					this.collisionElements.Add(collider, (ICollidable)behavior);
				}
			}
		}

		public void UnregisterAsCollider(FrameworkElement collider)
		{
			this.collidableElements.Remove(collider);
		}

		public void RegisterAsCollidable(FrameworkElement collidable)
		{
			foreach (Behavior behavior in Interaction.GetBehaviors(collidable))
			{
				if (behavior is ICollidable)
				{
					this.collidableElements.Add(collidable, (ICollidable)behavior);
					break;
				}
			}
		}

		public void UnregisterAsCollidable(FrameworkElement collidable)
		{
			this.collidableElementsToRemove.Add(collidable);
		}

		public void ChangeLevel(int level)
		{
			this.Level = level;
			if (this.LevelChanging != null)
			{
				this.LevelChanging(this, EventArgs.Empty);
			}
			this.HandleReset();
			bool success = VisualStateManager.GoToState(mainControl, "Level" + (((this.Level - 1) % NUM_LEVELS) + 1), true);
			if (!success || level > MAX_NUM_LEVELS)
			{
				this.EndGame();
			}
			else
			{
				VisualStateManager.GoToState(mainControl, "Reset", true);
				VisualStateManager.GoToState(mainControl, "WaitStart", true);
			}
		}

		#region Local Storage (High-score)

		private void SaveHighscore(int value)
		{
			IsolatedStorageFile score = IsolatedStorageFile.GetUserStoreForApplication();
			IsolatedStorageFileStream isfs = new IsolatedStorageFileStream("highscore.txt", FileMode.Create, score);
			StreamWriter streamWriter = new StreamWriter(isfs);
			streamWriter.WriteLine(value.ToString());
			streamWriter.Flush();
			streamWriter.Close();
		}

		private int ReadHighscore()
		{
			IsolatedStorageFile score = IsolatedStorageFile.GetUserStoreForApplication();
			string result;
			if (score.FileExists("highscore.txt"))
			{
				IsolatedStorageFileStream isfs = new IsolatedStorageFileStream("highscore.txt", FileMode.OpenOrCreate, score);
				StreamReader streamReader = new StreamReader(isfs);
				result = streamReader.ReadLine();
				streamReader.Close();
			}
			else
			{
				IsolatedStorageFileStream isfs = new IsolatedStorageFileStream("highscore.txt", FileMode.CreateNew, score);
				StreamWriter streamWriter = new StreamWriter(isfs);
				streamWriter.WriteLine("0");
				streamWriter.Flush();
				streamWriter.Close();
				result = "0";
			}
			return int.Parse(result);
		}

		#endregion
	}
}
