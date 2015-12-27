using System;
using System.Windows;

namespace WpfControls
{
	
	public partial class App : Application
	{
		public App() : base()
		{
			// TODO : this is apparently better - http://www.codeproject.com/Articles/846062/WPF-user-controls-do-not-use-merged-dictionaries-f
			// http://stackoverflow.com/a/9739610/4359306
			//Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(@"/CommonUI;component/Xaml/XamlControls.xaml", UriKind.Relative) });
		}
	}
}
