using CommonDialogs.Properties;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CommonDialogs.View
{
	public partial class DialogCheckboxList : UserControl, INotifyPropertyChanged
	{
		public bool okClicked;

		private List<DialogCheckBoxItem> _checkboxList;
		public List<DialogCheckBoxItem> checkboxList
		{
			get { return _checkboxList; }
			set { _checkboxList = value; OnPropertyChanged("checkboxList"); }
		}


		// Constructor
		public DialogCheckboxList()
		{
			InitializeComponent();			
        }


		// Ok Button Clicked
		private void Button_Click(object sender, RoutedEventArgs e)
		{			
			okClicked = true;
			((DialogMain) Window.GetWindow(this)).CloseDialog();
		}



		// Userform Visible
		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool? visible = (bool) e.NewValue;
			if (visible != null && visible == true)
			{
				okClicked = false;

				string previouslySelectedItems = Settings.Default.CheckboxListSelectedDataItems;
				List<string> items = previouslySelectedItems.Split('|').ToList();

				// Try to find previously selected items
				if (items != null || items.Count > 0)				
					for (int i = 0; i < checkboxList.Count; i++)
						foreach (string s in items)						
							if (s.Length > 0 && s == checkboxList[i].data)
								checkboxList[i].selected = true;

				ListBoxCheckboxes.ItemsSource = checkboxList;
			}
		}



		// Property Changed Event
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}
	}


	// Checkbox item
	public class DialogCheckBoxItem
	{
		public string displayName { get; set; }
		public string data { get; set; }
		public bool selected { get; set; }

		public DialogCheckBoxItem()
		{
			selected = false;
		}
	}
}
