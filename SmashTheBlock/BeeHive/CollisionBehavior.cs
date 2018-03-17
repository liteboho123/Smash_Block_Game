using System;
using System.Windows;
using System.Windows.Media;

namespace BeeHive
{
	public class CollisionBehavior : GameBehaviorBase<FrameworkElement>, ICollidable
	{
		protected override void OnGameEnvironmentInitialized()
		{
			this.GameEnvironment.RegisterAsCollidable(this.AssociatedObject);
			this.GameEnvironment.LevelChanging += this.OnLevelChanging;
			this.GameEnvironment.Reset += this.OnReset;
		}

		private void OnReset(object sender, EventArgs e)
		{
			this.UnregisterCollidableIfNecessary();
		}

		private void OnLevelChanging(object sender, EventArgs e)
		{
			this.UnregisterCollidableIfNecessary();
		}

		private void UnregisterCollidableIfNecessary()
		{
			// check to see if we're still in the tree and remove ourselves if not
			FrameworkElement element = this.AssociatedObject;
			while (element != null)
			{
				if (element.Equals(this.GameEnvironment.RootElement))
				{
					return;
				}
				element = VisualTreeHelper.GetParent(element) as FrameworkElement;
			}
			this.GameEnvironment.UnregisterAsCollidable(this.AssociatedObject);
		}

		#region ICollidable Members

		public Rect Position
		{
			get
			{
				Point upperLeft = this.AssociatedObject.TransformToVisual(this.GameEnvironment.RootElement).Transform(new Point(0, 0));
				return new Rect(upperLeft, new Size(this.AssociatedObject.ActualWidth, this.AssociatedObject.ActualHeight));
			}
		}

		#endregion
	}
}
