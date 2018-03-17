using System;
using System.Windows;
using System.Windows.Interactivity;

namespace BeeHive
{
	public abstract class GameTrigger<T> : EventTriggerBase<T> where T : FrameworkElement
	{
		private GameEnvironmentWalker gameEnvironmentWalker;

		protected GameEnvironment GameEnvironment
		{
			get { return this.gameEnvironmentWalker.GameEnvironment; }
		}

		protected override void OnSourceChanged(T oldSource, T newSource)
		{
			base.OnSourceChanged(oldSource, newSource);
			this.gameEnvironmentWalker = new GameEnvironmentWalker(this.Source);

			if (this.gameEnvironmentWalker.GameEnvironment != null)
			{
				this.OnGameEnvironmentInitialized();
			}
			else
			{
				this.gameEnvironmentWalker.GameEnvironmentLoaded += this.OnGameEnvironmentLoaded;
			}
		}

		private void OnGameEnvironmentLoaded(object sender, EventArgs e)
		{
			this.OnGameEnvironmentInitialized();
		}

		protected virtual void OnGameEnvironmentInitialized()
		{

		}

		protected override string GetEventName()
		{
			return string.Empty;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			this.gameEnvironmentWalker.GameEnvironmentLoaded -= this.OnGameEnvironmentLoaded;
			this.gameEnvironmentWalker = null;
		}
	}
}
