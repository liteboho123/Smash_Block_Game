using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace BeeHive
{
	public class MotionBehavior : GameBehaviorBase<FrameworkElement>
	{
		private bool isMoving;
		private double vo = 0;
		private double startX, startY;
		private double startDirection;
		private double startSpeed;

		#region Dependency Properties

		[Category("Motion Properties")]
		public double Speed { get { return (double)GetValue(SpeedProperty); } set { SetValue(SpeedProperty, value); } }
		[Category("Motion Properties")]
		public double Direction { get { return (double)GetValue(DirectionProperty); } set { SetValue(DirectionProperty, value); } }
		[Category("Motion Properties")]
		public bool AutoStart { get { return (bool)GetValue(AutoStartProperty); } set { SetValue(AutoStartProperty, value); } }
		
		public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register("Speed", typeof(double), typeof(MotionBehavior), new PropertyMetadata(null));
		public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(double), typeof(MotionBehavior), new PropertyMetadata(null));
		public static readonly DependencyProperty AutoStartProperty = DependencyProperty.Register("AutoStart", typeof(bool), typeof(MotionBehavior), new PropertyMetadata(null));

		#endregion

		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.Loaded += this.OnAssociatedObjectLoaded;
		}

		private MovableBehavior MovableBehavior
		{
			get;
			set;
		}

		private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
		{
			foreach (Behavior behavior in Interaction.GetBehaviors(this.AssociatedObject))
			{
				if (behavior is MovableBehavior)
				{
					this.MovableBehavior = (MovableBehavior)behavior;
					break;
				}
			}

			if (this.AutoStart)
			{
				this.vo = this.Speed;
				this.isMoving = true;
			}
			else
			{
				this.vo = 0;
				this.isMoving = false;
			}

			this.InitializeVelocity();
			this.startDirection = this.Direction;
			this.startSpeed = this.Speed;

			this.AssociatedObject.MouseLeftButtonUp += this.OnAssociatedObjectMouseUp;
		}

		// If Mouse Up, then start game
		private void OnAssociatedObjectMouseUp(object sender, MouseButtonEventArgs e)
		{
			if (this.vo == 0)
			{
				this.vo = this.Speed;
				this.InitializeVelocity();

				this.startX = this.MovableBehavior.X;
				this.startY = this.MovableBehavior.Y;

				this.isMoving = true;
			}
		}

		private void InitializeVelocity()
		{
			double vx = this.vo * Math.Cos(this.Direction * Math.PI / 180);
			double vy = this.vo * Math.Sin(this.Direction * Math.PI / 180);
			this.MovableBehavior.Velocity = new Vector(vx, vy);
		}

		protected override void OnGameEnvironmentInitialized()
		{
			this.GameEnvironment.RegisterAsCollider(this.AssociatedObject);
			this.GameEnvironment.GameLoop += this.OnGameLoop;
			this.GameEnvironment.Collision += this.GameEnvironment_Collision;
			this.GameEnvironment.LevelChanging += this.GameEnvironment_LevelChanging;
			this.GameEnvironment.Reset += this.GameEnvironment_Restart;
			this.GameEnvironment.GameOver += this.GameEnvironment_GameOver;
		}

		private void OnGameLoop(object sender, EventArgs e)
		{
			if (this.isMoving)
			{
				double vx = this.MovableBehavior.Velocity.X;
				double vy = this.MovableBehavior.Velocity.Y;

				this.MovableBehavior.SetPosition(new Point(this.MovableBehavior.X + vx, this.MovableBehavior.Y + vy));
			}
		}

		private void GameEnvironment_Collision(object sender, GameEnvironment.CollisionEventArgs e)
		{
			foreach (CollisionInformation collisionInformation in (from collisionInfo in e.CollisionInformation
																   where object.Equals(this.AssociatedObject, collisionInfo.CollidingElement)
																   select collisionInfo))
			{
				double vx = this.MovableBehavior.Velocity.X;
				double vy = this.MovableBehavior.Velocity.Y;
				Vector collisionNormal = collisionInformation.Normal;

				// move the object out of the collision region
				Point newPosition = new Point(this.MovableBehavior.X, this.MovableBehavior.Y);
				if (collisionNormal.X < 0)
				{
					newPosition.X -= 2 * vx;
				}
				if (collisionNormal.Y < 0)
				{
					newPosition.Y -= 2 * vy;
				}
				this.MovableBehavior.SetPosition(newPosition);

				// deflect the velocity around the normal of collision
				double dot = vx * collisionNormal.X + vy * collisionNormal.Y;
				vx -= (2 * dot * collisionNormal.X);
				vy -= (2 * dot * collisionNormal.Y);

				this.MovableBehavior.Velocity = new Vector(vx, vy);
				this.Direction = Math.Atan2(vy, vx) * 180 / Math.PI;
			}
		}

		private void GameEnvironment_GameOver(object sender, EventArgs e)
		{
			this.Speed = this.startSpeed;
		}

		private void GameEnvironment_Restart(object sender, EventArgs e)
		{
			// If life is lost, restart Motion
			this.vo = 0;
			this.isMoving = false;
			this.MovableBehavior.Velocity = new Vector(0, 0);
			this.MovableBehavior.SetPosition(new Point(this.startX, this.startY));
			this.Direction = this.startDirection;
		}

		private void GameEnvironment_LevelChanging(object sender, EventArgs e)
		{
			// Increase Speed each new Level
			this.Speed++;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			this.isMoving = false;
			this.AssociatedObject.Loaded -= this.OnAssociatedObjectLoaded;
			this.AssociatedObject.MouseLeftButtonUp -= this.OnAssociatedObjectMouseUp;
		}
	}
}
