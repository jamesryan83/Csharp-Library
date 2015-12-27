using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommonUI.Controls
{
	// https://social.msdn.microsoft.com/Forums/en-US/8ba13699-7f7f-4ab6-8e3e-f7d787355d81/image-button?forum=vswpfdesigner
	public class ImageButton : Button
	{
		public static readonly DependencyProperty ImageButtonSourceProperty;
		public static readonly DependencyProperty ClickCommandProperty;

		static ImageButton()
		{			
			ImageButtonSourceProperty = DependencyProperty.Register("ImageButtonSource", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));
			ClickCommandProperty = DependencyProperty.Register("ClickCommandSource", typeof(ICommand), typeof(ImageButton), new UIPropertyMetadata(null));
		}

		public ImageButton()
		{

		}
		
		public ImageSource ImageButtonSource
		{
			get { return (ImageSource) GetValue(ImageButtonSourceProperty); }
			set { SetValue(ImageButtonSourceProperty, value); }
		}

		public ICommand ClickCommandSource
		{
			get { return (ICommand) GetValue(ClickCommandProperty); }
			set { SetValue(ClickCommandProperty, value); }
		}
	}
}
