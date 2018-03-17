using System.Windows;
using System.Windows.Controls;

namespace BeeHive
{
	public class GamePropertyUpdateBehavior : GameBehaviorBase<TextBlock>
	{
		public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(GamePropertyUpdateBehavior), null);

		public string PropertyName
		{
			get { return (string)this.GetValue(PropertyNameProperty); }
			set { this.SetValue(PropertyNameProperty, value); }
		}

		protected override void OnGameEnvironmentInitialized()
		{
			this.AssociatedObject.Text = this.GameEnvironment.PropertyDictionary.GetValue<object>(this.PropertyName).ToString();
			this.GameEnvironment.PropertyDictionary.RegisterPropertyChangeHandler(this.PropertyName, this.OnPropertyChanged);
		}

		private void OnPropertyChanged(object oldValue, object newValue)
		{
			this.AssociatedObject.Text = this.GameEnvironment.PropertyDictionary.GetValue<object>(this.PropertyName).ToString();
		}
	}
}
