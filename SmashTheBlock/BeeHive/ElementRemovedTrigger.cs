using System;
using System.Windows;

namespace BeeHive
{
	public class ElementRemovedTrigger : GameTrigger<FrameworkElement>
	{
		protected override void OnGameEnvironmentInitialized()
		{
			this.GameEnvironment.ElementRemoved += this.OnElementRemoved;
		}

		private void OnElementRemoved(object sender, EventArgs args)
		{
			this.InvokeActions(null);
		}
	}
}
