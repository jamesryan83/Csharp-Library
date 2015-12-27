using System.Windows;
using System.Windows.Controls;

namespace CommonDialogs.View
{	
	public partial class DialogMessageBoxOkCancel : UserControl
	{
		public string message;
		public bool okClicked;

		public DialogMessageBoxOkCancel()
		{
			InitializeComponent();
		}

		// Userform Visible
		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool? visible = (bool) e.NewValue;
			if (visible != null && visible == true)
			{
				okClicked = false;
				TextBlockMessage.Text = message;
			}
		}


		private void ButtonOk_Click(object sender, RoutedEventArgs e)
		{
			okClicked = true;
			((DialogMain) Window.GetWindow(this)).CloseDialog();
		}


		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			okClicked = false;
			((DialogMain) Window.GetWindow(this)).CloseDialog();
		}
	}
}
