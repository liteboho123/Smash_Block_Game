using System;
using System.Windows;
using System.Windows.Interactivity;

namespace BeeHive
{
	public abstract class GameBehaviorBase<T> : Behavior<T> where T : FrameworkElement
	{
		private GameEnvironmentWalker gameEnvironmentWalker;

		protected GameEnvironment GameEnvironment
		{
			get
			{
				return this.gameEnvironmentWalker.GameEnvironment;
			}
		}

		public FrameworkElement Element
		{
			get { return this.AssociatedObject; }
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			this.gameEnvironmentWalker = new GameEnvironmentWalker(this.AssociatedObject);
			this.gameEnvironmentWalker.GameEnvironmentLoaded += this.OnGameEnvironmentLoaded;
		}

		private void OnGameEnvironmentLoaded(object sender, EventArgs e)
		{
			this.OnGameEnvironmentInitialized();
		}

		protected virtual void OnGameEnvironmentInitialized()
		{
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			this.gameEnvironmentWalker = null;
		}
	}
}
