using CommonDialogs.Properties;
using CommonUI.Behaviors;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CommonDialogs.View
{
	// Dialog Container Window
	public partial class DialogMain : Window
	{
		//WindowResizing windowResizing;  // doesn't work to well when calling dialog second time, keeps size of previous dialog
		WindowDragging windowDragging;

		private bool dialogWasClosed;

		#region Singleton

		private static DialogMain instance;

		// Constructor
		public DialogMain()
		{
			InitializeComponent();
		}

		public static DialogMain GetInstance()
		{
			if (instance == null)
				instance = new DialogMain();

            return instance;
		}

		#endregion

		
		#region Show Dialog

		// Show StringList dialog
		public void ShowStringListDialog(string title, List<string> stringList)
		{
			TextBlockTitle.Text = title;
			myDialogStringList.SetStringList(stringList);
			updateVisibleDialog("stringList");
			ShowDialog();
		}


		// Show Checkbox dialog
		public List<string> ShowCheckboxListDialog(string title, List<DialogCheckBoxItem> checkboxList, out bool wasClosed)
		{
			TextBlockTitle.Text = title;
			myDialogCheckboxList.checkboxList = checkboxList;
			updateVisibleDialog("checkBoxList");
			ShowDialog();

			List<string> selectedItemsData = null;

			// Get selected items
			if (myDialogCheckboxList.okClicked == true)
			{
				selectedItemsData = myDialogCheckboxList.checkboxList
					.Where(x => x.selected == true)
					.Select(x => x.data)
					.ToList();

				// Save to settings
				if (selectedItemsData != null && selectedItemsData.Count > 0)
				{
					Settings.Default.CheckboxListSelectedDataItems = string.Join("|", selectedItemsData);
					Settings.Default.Save();
				}
				else
				{
					Settings.Default.CheckboxListSelectedDataItems = "";
					Settings.Default.Save();
				}
			}

			wasClosed = dialogWasClosed;
			return selectedItemsData;
		}


		// Show SingleInput dialog
		public string ShowSingleInputDialog(string title, string textBoxLabel, out bool wasClosed, string defaultValue = "")
		{
			TextBlockTitle.Text = title;
			myDialogSingleInput.enteredValue = defaultValue;
			myDialogSingleInput.textBoxLabel = textBoxLabel;
			updateVisibleDialog("singleInput");
			ShowDialog();

			wasClosed = dialogWasClosed;
			return myDialogSingleInput.enteredValue;
		}


		// Show MessageBox OK
		public void ShowMessageBoxOk(string title, string message)
		{
			TextBlockTitle.Text = title;
			myDialogMessageBoxOk.message = message;
			updateVisibleDialog("messageBoxOk");
			ShowDialog();
		}


		// Show MessageBox OK Cancel
		public bool ShowMessageBoxOkCancel(string title, string message)
		{
			TextBlockTitle.Text = title;
			myDialogMessageBoxOkCancel.message = message;
			updateVisibleDialog("messageBoxOkCancel");
			ShowDialog();

			return myDialogMessageBoxOkCancel.okClicked;
		}

		#endregion


		#region Window Events

		// Window loaded event
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//if (windowResizing == null)
			//	windowResizing = new WindowResizing(this);

			if (windowDragging == null)
				windowDragging = new WindowDragging(this, false);
		}


		// Close from ok button or something other than close button
		public void CloseDialog()
		{
			dialogWasClosed = false;
			Hide();
		}


		// Close button
		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			dialogWasClosed = true;
			Hide();
		}

		#endregion
		

		// Set which dialog to display
		private void updateVisibleDialog(string dialog)
		{
			myDialogStringList.Visibility = Visibility.Collapsed;
			myDialogSingleInput.Visibility = Visibility.Collapsed;
			myDialogCheckboxList.Visibility = Visibility.Collapsed;
			myDialogMessageBoxOk.Visibility = Visibility.Collapsed;
			myDialogMessageBoxOkCancel.Visibility = Visibility.Collapsed;

			switch (dialog)
			{
				case "stringList":
					myDialogStringList.Visibility = Visibility.Visible;
					break;
				case "singleInput":
					myDialogSingleInput.Visibility = Visibility.Visible;
					break;
				case "checkBoxList":
					myDialogCheckboxList.Visibility = Visibility.Visible;
					break;
				case "messageBoxOk":
					myDialogMessageBoxOk.Visibility = Visibility.Visible;
					break;
				case "messageBoxOkCancel":
					myDialogMessageBoxOkCancel.Visibility = Visibility.Visible;
					break;
			}
		}
		
	}
}
