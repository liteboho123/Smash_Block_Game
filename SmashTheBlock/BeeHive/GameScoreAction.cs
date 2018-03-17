using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BeeHive
{
	public class GameScoreAction : GameTriggerAction<FrameworkElement>
	{
		private Popup popup;
		private Point parentOffset;

		private BlockControl BlockControl
		{
			get
			{
				FrameworkElement element = this.AssociatedObject;
				BlockControl control = element as BlockControl;
				while (element != null && control == null)
				{
					element = element.Parent as FrameworkElement;
					control = element as BlockControl;
				}
				return control;
			}
		}

		protected override void OnGameEnvironmentInitialized()
		{
			this.parentOffset = (this.BlockControl.TransformToVisual(this.GameEnvironment.RootElement).Transform(new Point(this.BlockControl.ActualWidth / 2.0, this.BlockControl.ActualHeight / 2.0)));
			this.parentOffset.X /= this.GameEnvironment.RootElement.RenderSize.Width;
			this.parentOffset.Y /= this.GameEnvironment.RootElement.RenderSize.Height;
		}

		protected override void Invoke(object parameter)
		{
			this.popup = new Popup();
			TextBlock textBlock = new TextBlock();
			textBlock.Text = this.BlockControl.ScoreValue.ToString();
			textBlock.FontSize = 16.0;
			textBlock.Foreground = new SolidColorBrush(Colors.White);
			textBlock.RenderTransform = new TranslateTransform();
			textBlock.Name = "textBlock";

			DoubleAnimation translationAnimation = new DoubleAnimation();
			translationAnimation.From = 0;
			translationAnimation.To = -25;
			translationAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

			DoubleAnimation opacityAnimation = new DoubleAnimation();
			opacityAnimation.From = 1.0d;
			opacityAnimation.To = 0.0d;
			opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

			popup.Child = textBlock;

			popup.HorizontalOffset = this.parentOffset.X * this.GameEnvironment.RootElement.RenderSize.Width;
			popup.VerticalOffset = this.parentOffset.Y * this.GameEnvironment.RootElement.RenderSize.Height;
			Point p = this.GameEnvironment.RootElement.TransformToVisual(Application.Current.RootVisual).Transform(new Point(0, 0));
			popup.HorizontalOffset += p.X;
			popup.VerticalOffset += p.Y;
			popup.IsOpen = true;

			Storyboard storyboard = new Storyboard();
			storyboard.Children.Add(translationAnimation);
			storyboard.Children.Add(opacityAnimation);
			storyboard.Duration = new Duration(TimeSpan.FromSeconds(1));

			Storyboard.SetTarget(translationAnimation, textBlock);
			Storyboard.SetTargetProperty(translationAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
			Storyboard.SetTarget(opacityAnimation, textBlock);
			Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
			storyboard.Completed += OnStoryboardCompleted;
			storyboard.Begin();
			this.GameEnvironment.PropertyDictionary.SetValue<int>("Score", this.GameEnvironment.PropertyDictionary.GetValue<int>("Score") + this.BlockControl.ScoreValue);
		}

		private void OnStoryboardCompleted(object sender, EventArgs e)
		{
			this.popup.IsOpen = false;
			this.popup = null;
		}
	}
}
