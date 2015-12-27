using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonUI.Controls
{
	public class TextBoxDragDrop : TextBox
	{	
		// Constructor
		public TextBoxDragDrop() : base()
		{
			this.AllowDrop = true;
			this.PreviewDrop += TextBox_Drop;
			this.PreviewDragOver += TextBox_DragOver;
			this.PreviewMouseDown += TextBox_MouseDown;
		}

		
		// Textbox drop
		public void TextBox_Drop(object sender, DragEventArgs e)
		{
			this.Text = WindowUtil.GetDataFromDropEvent(e)[0];
		}


		// Textbox dragover
		public void TextBox_DragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
			e.Effects = DragDropEffects.Copy;
		}


		// Textbox clicked
		public void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// triple click select all text
			if (e.ClickCount == 3)
				this.SelectAll();
		}
	}
}
