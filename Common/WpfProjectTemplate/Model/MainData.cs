using System.ComponentModel;

namespace WpfProjectTemplate.Model
{
	
	// Main Data Class
	class MainData : INotifyPropertyChanged
	{
		
		#region Get Singleton Instance of MainData class

		private static MainData instance;

		// Constructor
		protected MainData()
		{
			// MainData initialization code here...
		}

		// Get singleton instance
		public static MainData GetInstance()
		{
			if (instance == null)
				instance = new MainData();

			return instance;
		}

		#endregion



		// MainData variables, methods, event callbacks etc. here...




		// Property Changed Event
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}
	}
}
