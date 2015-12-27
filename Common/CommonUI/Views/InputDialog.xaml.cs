using System.Text.RegularExpressions;
using System.Windows;

namespace CommonUI.Views
{
	public partial class InputDialog : Window
	{

		public string newName = "";

		// Main
		public InputDialog(string labelText, string text)
		{
			InitializeComponent();

			LabelName.Content = labelText;
			TextBoxNewName.Text = text;
			TextBoxNewName.Focus();
			TextBoxNewName.SelectAll();
		}

		// Ok button
		private void ButtonOk_Click(object sender, RoutedEventArgs e)
		{
			if (TextBoxNewName.Text.Length == 0)
			{
				MessageBox.Show("You need to enter a value");
				return;
			}

			//if (Regex.Match(TextBoxNewName.Text, "^[A-Za-z0-9.-_ ]+$").Success == false)
			//{
			//	MessageBox.Show("You have entered invalid characters.  Only use A-Z, a-z, 0-9, . _ - and space");
			//	return;
			//}

			newName = TextBoxNewName.Text;
			this.Close();
		}

		// Textbox press Enter or Esc
		private void TextBoxNewName_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
				ButtonOk_Click(null, null);
			else if (e.Key == System.Windows.Input.Key.Escape)
			{
				newName = "";
				this.Close();
			}
		}
	}
}
