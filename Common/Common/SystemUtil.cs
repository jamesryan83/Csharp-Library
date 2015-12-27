using Shell32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Common
{
	public class SystemUtil
	{

		// Returns if the process is running or not (i.e. like is it in Task Manager window or not)
		public static bool IsSystemProcessRunning(string name)  // name = "EXCEL" when looking for Excel
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(name, "name", "IsSystemProcessRunning");

			List<string> processes = Process.GetProcesses().Select(x => x.ProcessName).ToList();
			if (processes.Contains(name))
				return true;
			else
				return false;
		}


		// Stops a system process, like clicking on End Process in Task Manager
		public static void KillSystemProcess(string processName)
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(processName, "processName", "KillSystemProcess");

			System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName(processName);
			foreach (System.Diagnostics.Process p in process)
			{
				if (string.IsNullOrEmpty(p.ProcessName) == false)
					try { p.Kill(); }
					catch { }
			}
		}


		// Returns with and height of Screens
		public static List<int[]> GetScreenSizes()
		{
			List<int[]> screenSizes = new List<int[]>();

			foreach (Screen s in Screen.AllScreens)
				screenSizes.Add(new int[] { s.WorkingArea.Width, s.WorkingArea.Height });

			return screenSizes;
		}


		// Opens a file or folder in windows explorer
		public static void OpenPathInWindowsExplorer(string fileOrFolderPath)
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(fileOrFolderPath, "fileOrFolderPath", "OpenPathInWindowsExplorer");

			if (FileFolderUtil.IsPathADirectory(fileOrFolderPath) == true)
				Process.Start(fileOrFolderPath);
			else
			{
				ArgumentUtil.IsFilePath(fileOrFolderPath, "fileOrFolderPath", "OpenPathInWindowsExplorer");
				Process.Start(fileOrFolderPath);
			}
		}


		// TODO : test
		// Run a file
		public static void RunFile(string filePath)
		{
			if (FileFolderUtil.IsPathADirectory(filePath) == true || File.Exists(filePath) == true)
				Process.Start(filePath);
			else
				throw new ExtendedArgumentException(filePath, "filePath", "RunFile", "You can only start a file or folder path");
		}		
	}
}
