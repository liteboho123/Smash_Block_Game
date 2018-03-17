using System.Windows;

namespace BeeHive
{
	public class GamePropertyChangeTrigger : GameTrigger<FrameworkElement>
	{
		public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(GamePropertyChangeTrigger), null);

		public string PropertyName
		{
			get { return (string)this.GetValue(PropertyNameProperty); }
			set { this.SetValue(PropertyNameProperty, value); }
		}

		protected override void OnGameEnvironmentInitialized()
		{
			this.GameEnvironment.PropertyDictionary.RegisterPropertyChangeHandler(this.PropertyName, this.PropertyChangeHandler);
		}

		private void PropertyChangeHandler(object oldValue, object newValue)
		{
			this.InvokeActions(null);
		}
	}
}
