using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace BeeHive
{
	/// <summary>
	/// This Action will move the element according to Mouse movement.
	/// </summary>
	public class FollowMouseBehavior : Behavior<FrameworkElement>
	{
		public enum PositionOptions 
		{ 
			None, 
			X, 
			Y, 
			XY 
		};

		// Private Properties
		private double maxX, maxY;
		private double minX, minY;

		private double width = 0;
		private double height = 0;
		private double oX = 0;
		private double oY = 0;

		// Vectors for Position and Velocity
		private Vector mousePosition = new Vector();
		private Vector targetPosition = new Vector();
		private Vector velocity = new Vector(0, 0);

		private UIElement parent;

		#region Dependency Properties

		public static readonly DependencyProperty FollowPositionProperty = DependencyProperty.Register("FollowPosition", typeof(PositionOptions), typeof(FollowMouseBehavior), null);
		public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(FollowMouseBehavior), null);

		[Category("Mouse Properties")]
		public PositionOptions FollowPosition 
		{ 
			get { return (PositionOptions)this.GetValue(FollowPositionProperty); } 
			set { this.SetValue(FollowPositionProperty, value); } 
		}

		[Category("Mouse Properties")]
		public Thickness Margin 
		{
			get { return (Thickness)this.GetValue(MarginProperty); } 
			set { this.SetValue(MarginProperty, value); } 
		}

		#endregion

		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.Loaded += this.OnAssociatedObjectLoaded;
		}

		private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
		{
			this.parent = VisualTreeHelper.GetParent(this.AssociatedObject) as UIElement;

			// Calculate Max/Min
			FrameworkElement container = parent as FrameworkElement;
			this.maxX = container.Width - this.AssociatedObject.Width - this.Margin.Right;
			this.minX = 0 + this.Margin.Left;
			this.maxY = container.Height - this.AssociatedObject.Height - this.Margin.Bottom;
			this.minY = 0 + this.Margin.Top;

			this.oX = this.AssociatedObject.RenderTransformOrigin.X;
			this.oY = this.AssociatedObject.RenderTransformOrigin.Y;
			this.width = this.AssociatedObject.Width;
			this.height = this.AssociatedObject.Height;

			this.targetPosition.X = Canvas.GetLeft(this.AssociatedObject) + (this.width * this.oX);
			this.targetPosition.Y = Canvas.GetTop(this.AssociatedObject) + (this.height * this.oX);

			container.MouseMove += this.HandleMouseMove;
		}

		private void HandleMouseMove(object sender, MouseEventArgs e)
		{
			UIElement reference = sender as UIElement;
			Point mouse = e.GetPosition(reference);

			double newX = mouse.X - (width * oX);
			double newY = mouse.Y - (height * oY);

			newX = Math.Min(Math.Max(newX, this.minX), this.maxX);
			newY = Math.Min(Math.Max(newY, this.minY), this.maxY);

			if (this.FollowPosition != PositionOptions.None)
			{
				switch (this.FollowPosition)
				{
					case PositionOptions.X:
						Canvas.SetLeft(this.AssociatedObject, newX);
						break;
					case PositionOptions.Y:
						Canvas.SetTop(this.AssociatedObject, newY);
						break;
					case PositionOptions.XY:
						Canvas.SetLeft(this.AssociatedObject, newX);
						Canvas.SetTop(this.AssociatedObject, newY);
						break;
				}
			}
		}
	}
}
