using CommonUI.Behaviors;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace WpfControls
{

	// This program is only for viewing xaml controls.  The controls are loaded in App.xaml from CommonUI/Xaml/XamlControls

	public partial class MainWindow : Window
	{
		WindowDragging windowDragging;
		
		public ObservableCollection<DataItem> items { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			Style = (Style) FindResource(typeof(Window));

			windowDragging = new WindowDragging(this, false);

			items = new ObservableCollection<DataItem>();

			for (int i = 0; i < 5; i++)
				items.Add(new DataItem { text = "Hi : " + i });

			SortableDragDropListBox.DataContext = this;
			SortableDragDropListBox.ItemsSource = items;

			DataContext = this;
		}
	}

	public class DataItem
	{
		public string text { get; set; }
	}
}
