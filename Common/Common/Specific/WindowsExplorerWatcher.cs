using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Specific
{
	public class WindowsExplorerWatcher
	{
		// Callback for other classes
		public delegate void WatcherUpdatedCallback(object sender, FileSystemEventArgs e);


		//https://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher%28v=vs.110%29.aspx
		// File system watcher
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public static void RunFileSystemWatcher(string filePath, WatcherUpdatedCallback callback)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "RunFileSystemWatcher");

			FileSystemWatcher watcher = new FileSystemWatcher(filePath, "*");
			watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size;
			watcher.Changed += new FileSystemEventHandler(callback);
			watcher.Created += new FileSystemEventHandler(callback);
			watcher.Deleted += new FileSystemEventHandler(callback);
			watcher.Renamed += new RenamedEventHandler(callback);
			watcher.IncludeSubdirectories = true;
			watcher.EnableRaisingEvents = true;
		}


		// Get file path properties
		private string GetSpecificFileProperties(string filePath, params int[] indexes)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "GetSpecificFileProperties");
			ArgumentUtil.IsNotNull(indexes, "indexes", "GetSpecificFileProperties");

			string fileName = Path.GetFileName(filePath);
			string folderName = Path.GetDirectoryName(filePath);
			Shell32.Shell shell = new Shell32.Shell();
			Shell32.Folder objFolder;
			objFolder = shell.NameSpace(folderName);
			StringBuilder sb = new StringBuilder();

			foreach (Shell32.FolderItem2 item in objFolder.Items())
			{
				if (fileName == item.Name)
				{
					for (int i = 0; i < indexes.Length; i++)
					{
						sb.Append(objFolder.GetDetailsOf(item, indexes[i]) + ",");
					}

					break;
				}
			}

			string result = sb.ToString().Trim();
			//Protection for no results causing an exception on the `SubString` method
			if (result.Length == 0)
			{
				return string.Empty;
			}
			return result.Substring(0, result.Length - 1);
		}
	}
}
