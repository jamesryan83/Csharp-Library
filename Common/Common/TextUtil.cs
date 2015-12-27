using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace Common
{
	public class TextUtil
	{
		
		// Splits a string and trims the results
		public static List<string> SplitAndTrimString(string stringToSplit, string delimiter)
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(stringToSplit, "stringToSplit", "SplitAndTrimString");
			ArgumentUtil.IsNotWhiteSpaceOrNull(delimiter, "delimiter", "SplitAndTrimString");

			return stringToSplit.Split(new string[] { delimiter }, StringSplitOptions.None)
				.Select(x => x.Trim())
				.ToList();
		}


		// Splits a string and trims the results and removes blanks
		public static List<string> SplitAndTrimStringAndRemoveBlanks(string stringToSplit, string delimiter)
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(stringToSplit, "stringToSplit", "SplitAndTrimStringAndRemoveBlanks");
			ArgumentUtil.IsNotWhiteSpaceOrNull(delimiter, "delimiter", "SplitAndTrimStringAndRemoveBlanks");

			return stringToSplit.Split(new string[] { delimiter }, StringSplitOptions.None)
				.Select(x => x.Trim())
				.Where(x => x.Length > 0)
				.ToList();
		}


		// Save some text to the Windows clipboard
		public static void SaveTextToWindowsClipboard(string text)
		{
			if (text != null)
				Clipboard.SetText(text);
		}


		// Returns the number of leading spaces in a string
		public int GetLeadingCharacters(char character, string text)
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(character.ToString(), "character", "GetLeadingCharacters");
			ArgumentUtil.IsNotWhiteSpaceOrNull(text, "text", "GetLeadingCharacters");

			int spaceCount = 0;
			if (text.Length > 0)
			{
				for (int j = 0; j < text.Length; j++)
					if (text[j] == character)
						spaceCount++;
					else
						break;
			}

			return spaceCount;
		}
	}
}
