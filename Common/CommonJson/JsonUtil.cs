using Newtonsoft.Json;
using System.IO;
using System;

namespace CommonJson
{
	public class JsonUtil
	{
		// Convert Object to Json String
		public static string ConvertObjectToJsonString(object data)
		{
			return JsonConvert.SerializeObject(data, Formatting.Indented);
		}


		// Convert Json string to object
		public static T ConvertJsonStringToObject<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}


		// Save data to json
		public static void SaveDataToJsonFile(object data, string filePath)
		{			
			if (filePath == null || filePath.Length == 0)
				return;

			using (StreamWriter writer = new StreamWriter(filePath))
			{
				string json = JsonConvert.SerializeObject(data, Formatting.Indented);
				writer.Write(json);
			}			
		}


		// TODO : crash here when data isn't right
		// Load data from json
		public static T LoadDataFromJsonFile<T>(string filePath) where T : class
		{
			if (filePath == null || filePath.Length == 0 || File.Exists(filePath) == false)
				return null;

			using (StreamReader reader = new StreamReader(filePath))
			{
				string json = reader.ReadToEnd();

				return JsonConvert.DeserializeObject<T>(json);		
			}			
		}
	}
}



