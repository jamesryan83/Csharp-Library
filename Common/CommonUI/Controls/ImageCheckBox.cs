using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommonUI.Controls
{
	// https://social.msdn.microsoft.com/Forums/en-US/8ba13699-7f7f-4ab6-8e3e-f7d787355d81/image-button?forum=vswpfdesigner
	public class ImageCheckBox : CheckBox
	{
		public static readonly DependencyProperty ImageCheckBoxSourceProperty;
		public static readonly DependencyProperty ClickCommandProperty;

		static ImageCheckBox()
		{
			ImageCheckBoxSourceProperty = DependencyProperty.Register("ImageCheckBoxSource", typeof(ImageSource), typeof(ImageCheckBox), new UIPropertyMetadata(null));
			ClickCommandProperty = DependencyProperty.Register("ClickCommandSource", typeof(ICommand), typeof(ImageCheckBox), new UIPropertyMetadata(null));
		}

		public ImageCheckBox()
		{

		}

		public ImageSource ImageCheckBoxSource
		{
			get { return (ImageSource) GetValue(ImageCheckBoxSourceProperty); }
			set { SetValue(ImageCheckBoxSourceProperty, value); }
		}

		public ICommand ClickCommandSource
		{
			get { return (ICommand) GetValue(ClickCommandProperty); }
			set { SetValue(ClickCommandProperty, value); }
		}
	}
}
