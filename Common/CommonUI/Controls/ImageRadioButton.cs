using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommonUI.Controls
{
	// https://social.msdn.microsoft.com/Forums/en-US/8ba13699-7f7f-4ab6-8e3e-f7d787355d81/image-button?forum=vswpfdesigner
	public class ImageRadioButton : RadioButton
	{
		public static readonly DependencyProperty ImageRadioButtonSourceProperty;
		public static readonly DependencyProperty ClickCommandProperty;

		static ImageRadioButton()
		{
			ImageRadioButtonSourceProperty = DependencyProperty.Register("ImageRadioButtonSource", typeof(ImageSource), typeof(ImageRadioButton), new UIPropertyMetadata(null));
			ClickCommandProperty = DependencyProperty.Register("ClickCommandSource", typeof(ICommand), typeof(ImageRadioButton), new UIPropertyMetadata(null));
		}

		public ImageRadioButton()
		{

		}

		public ImageSource ImageRadioButtonSource
		{
			get { return (ImageSource) GetValue(ImageRadioButtonSourceProperty); }
			set { SetValue(ImageRadioButtonSourceProperty, value); }
		}

		public ICommand ClickCommandSource
		{
			get { return (ICommand) GetValue(ClickCommandProperty); }
			set { SetValue(ClickCommandProperty, value); }
		}
	}
}
