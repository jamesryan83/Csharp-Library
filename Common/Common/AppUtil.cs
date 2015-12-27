using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Common
{
	public class AppUtil
	{

		
		// Returns the directory this application is in		
		public static string GetApplicationDirectory()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}


		
		// Returns the directory this application is in, minus bin\debug		
		public static string GetApplicationDirectoryDebug()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("bin\\Debug", "");
		}

				
		
		// TODO : can't catch exception
		// Do something after pausing for some time.
		// Call like this : Execute(() => { code here... }, 1000);		
		public static async void ExecuteActionAfterDelay(Action action, long timeoutInMilliseconds)
		{
			ArgumentUtil.IsValueGreaterThan(timeoutInMilliseconds, -2, "timeoutInMilliseconds", "ExecuteActionAfterDelay", 
				"Can't execute an Action with a Delay less than -1");

			await Task.Delay((int) timeoutInMilliseconds);
			action();
		}
	}
}
