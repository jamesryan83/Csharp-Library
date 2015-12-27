using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CommonDialogs.View
{
	public partial class DialogSingleInput : UserControl
	{
		public string textBoxLabel;
		public string enteredValue;
			

		// Constructor
		public DialogSingleInput()
		{
			InitializeComponent();

			Style = (Style) FindResource(typeof(UserControl));
		}



		// Userform Visible
		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool? visible = (bool) e.NewValue;
            if (visible != null && visible == true)
			{				
				TextBoxInput.Text = enteredValue;

				// fixes problem setting focus on textbox
				// http://stackoverflow.com/a/13955730
				Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate () {
					TextBoxInput.Focus();
					Keyboard.Focus(TextBoxInput);
				}));
			}
		}



		// Ok Button Clicked
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			enteredValue = TextBoxInput.Text;
			((DialogMain) Window.GetWindow(this)).CloseDialog();
		}



		// Enter pressed on textbox
		private void TextBoxInput_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				enteredValue = TextBoxInput.Text;
				((DialogMain) Window.GetWindow(this)).CloseDialog();
			}
		}
	}
}
