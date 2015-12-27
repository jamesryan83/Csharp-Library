using System.IO;
using System.Linq;

namespace Common
{
    public class FileFolderUtil
	{	
		
		#region File/Folder Info

		// TODO : fix duplication, GetAllFilePaths & GetAllFolderPaths
		// TODO : make the checking of the *. at the start of fileExtensionFilter a bit more intelligent
		// Returns a list of files
		// fileExtensionFilter should only be letters, don't include * or .  Default value is all files
		public static string[] GetAllFilePaths(string folderPath, string fileExtensionFilter = null, bool recursiveSearch = false)
		{
			ArgumentUtil.IsFolderPath(folderPath, "folderPath", "GetAllFilePaths");

			if (fileExtensionFilter == null)
				fileExtensionFilter = "*";

			fileExtensionFilter = "*." + fileExtensionFilter;

			// TODO : unauthorized access with that $Recycle bin thing again when trying on C:\
			return Directory.GetFiles(folderPath, fileExtensionFilter,
				recursiveSearch == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		}


		// Returns a list of directories
		public static string[] GetAllFolderPaths(string folderPath, bool recursiveSearch = false)
		{
			ArgumentUtil.IsFolderPath(folderPath, "folderPath", "GetAllFolderPaths");

			return Directory.GetDirectories(folderPath, "*", recursiveSearch == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		}


		// TODO : is there a better way to do this ??
		// Returns true if a path is a directory, otherwise false
		public static bool IsPathADirectory(string path)
		{
			try
			{
				return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
			}
			catch
			{
				return false;
			}
		}


		// Get the size of the directory
		public static long GetDirectorySize(string path)
		{
			if (path == null || path.Length == 0 || IsPathADirectory(path) == false)
				return -1;

			DirectoryInfo baseDirectoryInfo = new DirectoryInfo(path);

			long size = 0;

			FileInfo[] fileInfos = baseDirectoryInfo.GetFiles();
			foreach (FileInfo info in fileInfos)
				size += info.Length;

			DirectoryInfo[] directoryInfos = baseDirectoryInfo.GetDirectories();
			foreach (DirectoryInfo info in directoryInfos)
				size += GetDirectorySize(info.FullName);
			
			return size;
		}


		// Get the name of a directory
		public static string GetDirectoryName(string path)
		{
			if (path == null || path.Length == 0)
				return null;

			return path.Substring(path.LastIndexOf('\\') + 1);
		}

		#endregion


		#region Create/Delete/Rename Files & Folders

		// Create a new folder with an incrementing end number
		public static void CreateNewIncrementingFolder(string containingFolderPath, string folderName = "New Folder")
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(folderName, "folderName", "CreateNewIncrementingFolder", "Error creting new folder");
			ArgumentUtil.IsFolderPath(containingFolderPath, "containingFolderPath", "CreateNewIncrementingFolder");

			string[] folders = GetAllFolderPaths(containingFolderPath);

			int count = folders
				.Where(x => 
					{
						string existingFolderName = GetDirectoryName(x);

						if (existingFolderName == folderName)
							return true;
						else if (existingFolderName.Contains(' '))
						{
							string temp = existingFolderName.Substring(0, existingFolderName.LastIndexOf(' '));
							return folderName == temp;
						}
						else
							return false;
					})
				.Count();

			if (count > 0)
				folderName = folderName + " " + (count + 1);

			Directory.CreateDirectory(Path.Combine(containingFolderPath, folderName));
		}

		
		// TODO : check for invalid characters in newName
		// Rename a folder
		public static bool RenameFolder(string currentPath, string newName)
		{
			ArgumentUtil.IsFolderPath(currentPath, "currentPath", "RenameFolder");
			ArgumentUtil.IsNotWhiteSpaceOrNull(newName, "newName", "RenameFolder", "Error renaming folder");
			
			string temp = currentPath.Substring(0, currentPath.LastIndexOf(Path.DirectorySeparatorChar));
			string newPath = Path.Combine(temp, newName);

			if (Directory.Exists(newPath))
				return false;

			Directory.Move(currentPath, newPath);
			return true;
		}


		
		// Copy a folder with its files.  If desination folder exists, delete it first
		// https://msdn.microsoft.com/en-us/library/bb762914(v=vs.110).aspx
		public static void CopyFolderWithFiles(string sourceDirName, string destDirName, bool copySubDirs = true)
		{
			ArgumentUtil.IsFolderPath(sourceDirName, "sourceDirName", "CopyFolderWithFiles");			

			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] dirs = dir.GetDirectories();

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			// If the destination directory doesn't exist, create it. 
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, true);
			}

			// If copying subdirectories, copy them and their contents to new location. 
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					CopyFolderWithFiles(subdir.FullName, temppath, copySubDirs);
				}
			}
		}


		// Delete folder
		public static void DeleteFolder(string folderPath)
		{
			ArgumentUtil.IsFolderPath(folderPath, "folderPath", "DeleteFolder");
			
			Directory.Delete(folderPath, true);
		}


		// Delete file
		public static void DeleteFile(string filePath)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "DeleteFile");
			
			File.Delete(filePath);			
		}

		// TODO : check for invalid characters in newFileNameWithExtension
		// Rename a file
		public static void RenameFile(string currentFilePath, string newFileNameWithExtension)
		{
			ArgumentUtil.IsFilePath(currentFilePath, "currentFilePath", "RenameFile");
			ArgumentUtil.IsNotWhiteSpaceOrNull(newFileNameWithExtension, "newFileNameWithExtension", "RenameFile");

			string temp = currentFilePath.Substring(0, currentFilePath.LastIndexOf(Path.DirectorySeparatorChar));
			string newPath = Path.Combine(temp, newFileNameWithExtension);			

			File.Move(currentFilePath, newPath);			
		}


		// Move a file
		public static void MoveFile(string currentFilePath, string newFolderPath)
		{
			ArgumentUtil.IsFilePath(currentFilePath, "currentFilePath", "MoveFile");
			ArgumentUtil.IsFolderPath(newFolderPath, "newFolderPath", "MoveFile");
			
			File.Move(currentFilePath, Path.Combine(newFolderPath, Path.GetFileName(currentFilePath)));			
		}

		#endregion

	}
}
