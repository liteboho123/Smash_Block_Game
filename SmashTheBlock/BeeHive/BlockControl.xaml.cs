using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BeeHive
{
	public partial class BlockControl : UserControl
	{
		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(BlockControl), null);
		public static readonly DependencyProperty ScoreValueProperty = DependencyProperty.Register("ScoreValue", typeof(int), typeof(BlockControl), new PropertyMetadata(0));
		
		public ImageSource ImageSource
		{
			get { return (ImageSource)this.GetValue(ImageSourceProperty); }
			set { this.SetValue(ImageSourceProperty, value); }
		}

		public int ScoreValue
		{
			get { return (int)this.GetValue(ScoreValueProperty); }
			set { this.SetValue(ScoreValueProperty, value); }
		}

		public BlockControl()
		{
			this.InitializeComponent();
		}
	}
}