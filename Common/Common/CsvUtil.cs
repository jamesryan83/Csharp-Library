using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common
{
	public class CsvUtil
	{

		// Returns headers from a csv
		public static string[] GetHeadersFromCsv(string filePath, string delimiter = ",")
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "ReadHeadersFromCsv");

			if (delimiter == null || delimiter.Length == 0)
				delimiter = ",";

			try
			{
				using (StreamReader reader = new StreamReader(filePath))
				{
					return reader.ReadLine().Split(new string[] { delimiter }, StringSplitOptions.None);
				}
			}
			catch { return null; } // StreamReader will throw an Exception if the file is already open
		}


		// TODO : should this throw something when streamreader fails ?
		// Returns everthing except the headers from a csv - if hasHeaders = true, the first row will be skipped
		public static List<string[]> GetDataFromCsv(string filePath, string delimiter = ",", bool hasHeaders = true, bool ignoreEmptyRows = false)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "GetDataFromCsv"); 
						
			if (delimiter == null || delimiter.Length == 0)
				delimiter = ",";

			List<string[]> csvData = new List<string[]>();

			try
			{
				using (StreamReader reader = new StreamReader(filePath))
				{
					if (hasHeaders == true) // skip headers
						reader.ReadLine();

					while (reader.EndOfStream == false)  // split each csv line into string[]
						csvData.Add(reader.ReadLine().Split(new string[] { delimiter }, StringSplitOptions.None));
				}
			}
			catch { return null; }


			// This removes rows that are like ,,,,,,,,  from the list, as in, rows in the csv with just the delimiter (i.e. rows with no values)
			if (ignoreEmptyRows == true)
				csvData = csvData.Where(x => 
				{
					// if all values for a row are empty, return false, else true.  True adds row to new list, false ignores it.
					return x.Count(y => y.Length == 0) == x.Length  ? false : true;
				}).Select(x => x).ToList();

			return csvData;
		}

	}
}
