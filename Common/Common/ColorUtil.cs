using System.Drawing;

namespace Common
{
	public class ColorUtil
	{
		
		// Some distinct colors
		// http://graphicdesign.stackexchange.com/a/3686
		public readonly static string[] SomeColors = 
		{ 
			"#FFF6DFC6", "#FFFFFFC1", "#FFAFFFAF", "#FFB9FFFF", "#FF8BE1FF", "#FFC5C5FF", "#FFFFC5E2",
			"#FFEBB981", "#FFFFFF66", "#FF66FF66", "#FF66FFFF", "#FF33CCFF", "#FF9999FF", "#FFFF99CC", 
			"#FFD98121", "#FFF0EA00", "#FF00DE00", "#FF00DBD6", "#FF008EC0", "#FF2D2DFF", "#FFF6007B"
		};




		public static readonly char[] hexDigits = 
		{
			'0', '1', '2', '3', '4', '5', '6', '7',
			'8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
		};


		// Returns a Hex String from a System.Drawing.Color
		// http://stackoverflow.com/a/12812292
		public static string ColorToHexString(Color color, bool includeLeadingHashSymbol = true)
		{
			byte[] bytes = new byte[3];
			bytes[0] = color.R;
			bytes[1] = color.G;
			bytes[2] = color.B;

			char[] chars = new char[bytes.Length * 2];

			for (int i = 0; i < bytes.Length; i++)
			{
				int b = bytes[i];
				chars[i * 2] = hexDigits[b >> 4];
				chars[i * 2 + 1] = hexDigits[b & 0xF];
			}
			string hexString = new string(chars);

			if (includeLeadingHashSymbol == false)
				return hexString;
			else
				return "#" + hexString;
		}


	}
}
