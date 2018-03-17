using System.Windows;
using System.Windows.Controls;

namespace BeeHive
{
	public class ModifyGamePropertyAction : GameTriggerAction<TextBlock>
	{
		public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(ModifyGamePropertyAction), null);
		public static readonly DependencyProperty IncrementValueProperty = DependencyProperty.Register("IncrementValue", typeof(int), typeof(ModifyGamePropertyAction), null);

		public string PropertyName
		{
			get { return (string)this.GetValue(PropertyNameProperty); }
			set { this.SetValue(PropertyNameProperty, value); }
		}

		public int IncrementValue
		{
			get { return (int)this.GetValue(IncrementValueProperty); }
			set { this.SetValue(IncrementValueProperty, value); }
		}

		protected override void Invoke(object parameter)
		{
			try
			{
				int currentValue = this.GameEnvironment.PropertyDictionary.GetValue<int>(this.PropertyName);
				currentValue += this.IncrementValue;
				this.GameEnvironment.PropertyDictionary.SetValue<int>(this.PropertyName, currentValue);
			}
			catch
			{
			}
		}
	}
}
