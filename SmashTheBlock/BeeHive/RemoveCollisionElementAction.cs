using System.Windows;
using System.Windows.Controls;

namespace BeeHive
{
	public class RemoveCollisionElementAction : GameTriggerAction<FrameworkElement>
	{
		protected override void Invoke(object parameter)
		{
			Panel parentPanel = this.AssociatedObject.Parent as Panel;
			if (parentPanel != null)
			{
				parentPanel.Children.Remove(this.AssociatedObject);
			}
			this.GameEnvironment.UnregisterAsCollidable(this.AssociatedObject);
		}
	}
}
