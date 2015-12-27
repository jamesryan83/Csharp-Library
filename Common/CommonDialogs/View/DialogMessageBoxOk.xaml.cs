using System.Windows;
using System.Windows.Controls;

namespace CommonDialogs.View
{
	public partial class DialogMessageBoxOk : UserControl
	{
		public string message;

		public DialogMessageBoxOk()
		{
			InitializeComponent();
		}

		// Userform Visible
		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool? visible = (bool) e.NewValue;
			if (visible != null && visible == true)
			{
				TextBlockMessage.Text = message;
			}
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			((DialogMain) Window.GetWindow(this)).CloseDialog();
		}
	}
}
