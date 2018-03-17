using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Resources;

namespace BeeHive
{
	public partial class LevelHostControl : UserControl
	{
		public static readonly DependencyProperty HostFileProperty = DependencyProperty.Register("HostFile", typeof(object), typeof(LevelHostControl), new PropertyMetadata(null, new PropertyChangedCallback(OnHostFileChanged)));
		
		public string HostFile
		{
			get { return (string)this.GetValue(HostFileProperty); }
			set { this.SetValue(HostFileProperty, value); }
		}

		private static void OnHostFileChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			LevelHostControl hostControl = (LevelHostControl)sender;
			string newValue = (string)e.NewValue;

			if (!string.IsNullOrEmpty(newValue))
			{
				Uri uri = new Uri("/BeeHive;component/" + newValue, UriKind.Relative);
				StreamResourceInfo streamResourceInfo = Application.GetResourceStream(uri);
				StreamReader sr = new StreamReader(streamResourceInfo.Stream);

				object loadedLevel = XamlReader.Load(sr.ReadToEnd());
				hostControl.PART_CONTENT.Content = loadedLevel;
			}
		}
		
		public LevelHostControl()
		{
			// Required to initialize variables
			this.InitializeComponent();
		}
	}
}