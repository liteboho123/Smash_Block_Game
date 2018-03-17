using System;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace BeeHive
{
	public class GameEnvironmentWalker
	{
		private GameEnvironment gameEnvironment;
		public event EventHandler GameEnvironmentLoaded;

		public GameEnvironment GameEnvironment
		{
			get { return this.gameEnvironment; }
		}

		public GameEnvironmentWalker(FrameworkElement contextElement)
		{
			if (contextElement.Parent != null || Application.Current.RootVisual != null && Application.Current.RootVisual.Equals(contextElement))
			{
				this.FindGameEnvironment(contextElement);
			}
			else
			{
				contextElement.Loaded += this.OnContextElementLoaded;
			}
		}

		private void OnContextElementLoaded(object sender, RoutedEventArgs e)
		{
			this.FindGameEnvironment((FrameworkElement)sender);
		}

		private void FindGameEnvironment(FrameworkElement contextElement)
		{
			FrameworkElement parent = contextElement;
			while (parent != null)
			{
				BehaviorCollection behaviors = Interaction.GetBehaviors(parent);

				this.gameEnvironment = (GameEnvironment)(from behavior in behaviors
														 where typeof(GameEnvironment).IsAssignableFrom(behavior.GetType())
														 select behavior).FirstOrDefault();

				if (this.gameEnvironment != null)
				{
					if (this.GameEnvironmentLoaded != null)
					{
						this.GameEnvironmentLoaded(this, EventArgs.Empty);
					}
					break;
				}
				parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
			}
		}
	}
}
