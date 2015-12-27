using System.Windows;
using System.Windows.Controls;

namespace WpfProjectTemplate.View
{
	public partial class MainWindowContent : UserControl
	{
		public MainWindowContent()
		{
			InitializeComponent();

			Style = (Style) FindResource(typeof(UserControl));
		}
	}
}
