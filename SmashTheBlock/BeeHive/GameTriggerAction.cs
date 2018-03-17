using System;
using System.Windows;
using System.Windows.Interactivity;

namespace BeeHive
{
	public abstract class GameTriggerAction<T> : TriggerAction<T> where T : FrameworkElement
	{
		private GameEnvironmentWalker gameEnvironmentWalker;

		public GameEnvironment GameEnvironment
		{
			get { return this.gameEnvironmentWalker.GameEnvironment; }
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			this.gameEnvironmentWalker = new GameEnvironmentWalker(this.AssociatedObject);
			this.gameEnvironmentWalker.GameEnvironmentLoaded += this.OnGameEnvironmentLoaded;
		}

		protected virtual void OnGameEnvironmentInitialized()
		{
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			this.gameEnvironmentWalker.GameEnvironmentLoaded -= this.OnGameEnvironmentLoaded;
			this.gameEnvironmentWalker = null;
		}

		private void OnGameEnvironmentLoaded(object sender, EventArgs e)
		{
			this.OnGameEnvironmentInitialized();
		}
	}
}
