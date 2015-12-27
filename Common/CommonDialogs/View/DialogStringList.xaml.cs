using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CommonDialogs.View
{
	public partial class DialogStringList : UserControl
	{
		// Constructor
		public DialogStringList()
		{
			InitializeComponent();

			Style = (Style) FindResource(typeof(UserControl));
        }

		// Set the data in the string list
		public void SetStringList(List<string> stringList)
		{
			ListBoxStringList.ItemsSource = stringList;
		}
	}
}
