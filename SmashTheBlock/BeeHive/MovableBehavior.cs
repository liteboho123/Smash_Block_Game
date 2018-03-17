using System.Windows;
using System.Windows.Media;

namespace BeeHive
{
	public class MovableBehavior : GameBehaviorBase<FrameworkElement>, ICollidable
	{
		public double X
		{
			get { return this.Position.X; }
			set { this.SetPosition(new Point(value, this.Position.Y)); }
		}

		public double Y
		{
			get { return this.Position.Y; }
			set { this.SetPosition(new Point(this.Position.X, value)); }
		}

		public Vector Velocity
		{
			get;
			set;
		}

		public void SetPosition(Point p)
		{
			double diffX = p.X - this.X;
			double diffY = p.Y - this.Y;
			MatrixTransform matrixTransform = new MatrixTransform();
			TransformGroup group = new TransformGroup();
			TranslateTransform translateTransform = new TranslateTransform();
			if (this.AssociatedObject.RenderTransform != null)
			{
				group.Children.Add(this.AssociatedObject.RenderTransform);
			}
			translateTransform.X = diffX;
			translateTransform.Y = diffY;
			group.Children.Add(translateTransform);
			matrixTransform.Matrix = group.Value;
			this.AssociatedObject.RenderTransform = matrixTransform;
			this.AssociatedObject.UpdateLayout();
		}

		public Rect Position
		{
			get
			{
				Point position = this.AssociatedObject.TransformToVisual(this.GameEnvironment.RootElement).Transform(new Point(0, 0));
				return new Rect(position, new Size(this.AssociatedObject.ActualWidth, this.AssociatedObject.ActualHeight));
			}
		}
	}
}
