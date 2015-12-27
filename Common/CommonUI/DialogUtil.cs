using CommonUI.Views;
using System;
using System.IO;
using System.Windows;

namespace CommonUI
{
	public class DialogUtil
	{

		// Open File Dialog
		public static bool ShowOpenFileDialog(out string filePath, bool multiSelect = false, string filter = null)
		{
			Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

			dialog.Multiselect = multiSelect;

            if (filter != null)
				dialog.Filter = filter;

			Nullable<bool> result = dialog.ShowDialog();

			if (multiSelect == false)
				filePath = result == true ? dialog.FileName : "";
			else
				filePath = result == true ? string.Join("|", dialog.FileNames) : "";
			
			return result ?? false;
		}


		// TODO : Test
		// Save File Dialog
		public static bool ShowSaveFileDialog(out string filePath, string initialDirectory = null, string title = null, string filter = null)
		{
			Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();

			if (title != null) dialog.Title = title;
			if (initialDirectory != null && Directory.Exists(initialDirectory))
				dialog.InitialDirectory = initialDirectory;

			if (filter != null)
				dialog.Filter = filter;

			Nullable<bool> result = dialog.ShowDialog();
			filePath = result == true ? dialog.FileName : "";
			return result ?? false;
		}


		// Show Yes/No Message Box
		public static bool ShowYesNoMessageBox(string message, string title)
		{
			MessageBoxResult result = System.Windows.MessageBox.Show(message ?? "", title ?? "", MessageBoxButton.YesNo);
			return result == MessageBoxResult.Yes ? true : false;
		}


		// Show input dialog
		public static void ShowInputDialog(string caption, string defaultInput = "", Window window = null)
		{
			InputDialog dialog = new InputDialog(caption, defaultInput);
			if (window != null)
				dialog.Owner = window;
		}



		//	requires Ookii.Dialogs.Wpf
		//// This is a good looking folder dialog, instead of crappy .net one
		//public static bool ShowFolderBrowserDialog(Window window, out string folderPath, string title = "Please Select a Folder")
		//{		
		//	VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();		
		//	dialog.UseDescriptionForTitle = true;

		//	if (dialog.ShowDialog() == true)
		//	{
		//		folderPath = dialog.SelectedPath;
		//		return true;
		//	}
		//	else
		//	{
		//		folderPath = null;
		//		return false;
		//	}
		//}

	}
}
