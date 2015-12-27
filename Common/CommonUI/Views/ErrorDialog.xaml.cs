using Common;
using CommonUI.Behaviors;
using System.ComponentModel;
using System.Windows;

namespace CommonUI.Views
{
	/* How to use
	  
	 put this in App.cs to show the dialog ...
	  
	public App() : base()
	{
		this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
	}

	void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) 
	{
		ErrorDialog dialog = new ErrorDialog(e.Exception as ExtendedArgumentException);
		dialog.ShowDialog();

		e.Handled = true;
	}
	  
	  
	  and put this in app.xaml for the error icon ...
	  
	  <ResourceDictionary>
			<ResourceDictionary.MergedDictionaries>
				<ResourceDictionary Source="pack://application:,,,/CommonUI;component/Xaml/XamlImages.xaml" />
			</ResourceDictionary.MergedDictionaries>
		</ResourceDictionary>
	 */


	public partial class ErrorDialog : Window, INotifyPropertyChanged
	{
		private WindowDragging windowDragging;

		private ExtendedArgumentException _exception;
		public ExtendedArgumentException exception 
		{
			get { return _exception; } 
			set { _exception = value; OnPropertyChanged("e"); }
		}

		private Visibility _isAdvancedVisible;
		public Visibility isAdvancedVisible
		{
			get { return _isAdvancedVisible; }
			set { _isAdvancedVisible = value; OnPropertyChanged("isAdvancedVisible"); }
		}
		


		// Constructor
		public ErrorDialog(ExtendedArgumentException exception)
		{
			InitializeComponent();
						
			exception.stackTrace = exception.StackTrace;
			
			DataContext = this;
			isAdvancedVisible = System.Windows.Visibility.Collapsed;
			this.exception = exception;

			windowDragging = new WindowDragging(this, false);
		}

		
		// Property Changed Event
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}


		// Button Advanced
		private void ButtonAdvanced_Click(object sender, RoutedEventArgs e)
		{
			if (isAdvancedVisible == System.Windows.Visibility.Visible)
				isAdvancedVisible = System.Windows.Visibility.Collapsed;
			else
				isAdvancedVisible = System.Windows.Visibility.Visible;
		}


		// Button OK
		private void ButtonOk_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

	}
}
