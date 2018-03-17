using System.Windows.Controls;

namespace BeeHive
{
	public class GoToNextLevelAction: GameTriggerAction<Panel>
	{
		private bool isEnabled = true;

		protected override void Invoke(object parameter)
		{
			if (this.isEnabled)
			{
				if (this.AssociatedObject.Children.Count == 0)
				{
					this.GameEnvironment.ChangeLevel(this.GameEnvironment.Level + 1);
					this.isEnabled = false;
				}
			}
		}
	}
}
