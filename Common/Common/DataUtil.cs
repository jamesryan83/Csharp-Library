using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Common
{
	public class DataUtil
	{

		// Returns an empty datatable
		public static DataTable GetDataTable(List<string> columnNames)
		{
			ArgumentUtil.IsNotNull<List<string>>(columnNames, "columnNames", "GetDataTable");

			DataTable table = new DataTable();
			foreach (string s in columnNames)
				table.Columns.Add(s);

			return table;
		}


		// Get a byte[] from filepath
		public static byte[] GetByteArrayFromFile(string filePath)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "GetByteArrayFromFile");

			return File.ReadAllBytes(filePath);
		}


		// Returns true if two lists have same elements in the same order, otherwise false
		public static bool AreTheseTwoListsTheSame<T>(List<T> list1, List<T> list2)
		{
			ArgumentUtil.IsNotNull<List<T>>(list1, "list1", "AreTheseTwoListsTheSame");
			ArgumentUtil.IsNotNull<List<T>>(list2, "list2", "AreTheseTwoListsTheSame");

			return Enumerable.SequenceEqual(list1.OrderBy(x => x), list2.OrderBy(x => x));
		}


		#region Read & Write string & List<string> to & from file

		// Save string to file
		public static bool WriteStringToFile(string data, string filePath)
		{
			ArgumentUtil.IsNotNull<string>(data, "data", "WriteStringToFile");
			ArgumentUtil.IsNotWhiteSpaceOrNull(filePath, "filePath", "WriteStringToFile");

			try
			{
				using (StreamWriter writer = new StreamWriter(filePath))
				{
					writer.Write(data);
					return true;
				}
			}
			catch { return false; }  // StreamWriter will throw an Exception if the file is already open
		}


		// Save a list of strings to a file
		public static bool WriteStringListToFile(List<string> data, string filePath)
		{
			ArgumentUtil.IsNotNull<List<string>>(data, "data", "WriteStringListToFile");
			ArgumentUtil.IsNotWhiteSpaceOrNull(filePath, "filePath", "WriteStringListToFile");

			try
			{
				using (StreamWriter writer = new StreamWriter(filePath))
				{
					foreach (string s in data)
						writer.WriteLine(s);

					return true;
				}
			}
			catch { return false; }
		}


		// Read string from file
		public static string ReadStringFromFile(string filePath)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "ReadStringFromFile");

			try
			{
				using (StreamReader reader = new StreamReader(filePath))
				{
					return reader.ReadToEnd();
				}
			}
			catch { return null; }
		}


		// Read string list from file
		public static List<string> ReadStringListFromFile(string filePath)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "ReadStringListFromFile");

			try
			{
				using (StreamReader reader = new StreamReader(filePath))
				{
					List<string> list = new List<string>();
					while (reader.EndOfStream == false)
						list.Add(reader.ReadLine());

					return list;
				}
			}
			catch { return null; }
		}



		// save crash log
		// This creates an Error Logs folder in the exe folder		
		public static void WriteCrashToErrorLogsFolder<T>(T exception, string message = null) where T : Exception
		{
			// Create error logs directory if it doesn't exist
			string directory = Path.Combine(AppUtil.GetApplicationDirectory(), "Error Logs");
			if (Directory.Exists(directory) == false)
				Directory.CreateDirectory(directory);

			// Save data to file
			using (StreamWriter writer = new StreamWriter(Path.Combine(directory, "error_log_" + DateTime.Now.Ticks + ".txt")))
			{
				if (message != null && message.Length > 0)
					writer.Write("Message : " + message);

				writer.WriteLine();
				writer.WriteLine();
				writer.Write(exception.Message + "\n\n" + exception.StackTrace);
				writer.WriteLine();
				writer.WriteLine();
				writer.Write(Environment.StackTrace);
			}
		}

		#endregion
	}
}
