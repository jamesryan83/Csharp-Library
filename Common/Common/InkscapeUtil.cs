using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml;

namespace Common
{
	public class InkscapeUtil
	{
		// TODO : replace the template with a string in here instead
		// Creates a new svg file where all the paths are now on their own layers
		public static void MovePathsToLayers(string inputSvgFilePath, string templateSvgFilePath, string outputSvgFilePath)
		{
			ArgumentUtil.IsFilePath(inputSvgFilePath, "inputSvgFilePath", "MovePathsToLayers");
			ArgumentUtil.IsFilePath(templateSvgFilePath, "templateSvgFilePath", "MovePathsToLayers");						

			// the file to get paths from and file to put the layers into
			XmlDocument docMyFile = new XmlDocument();
			XmlDocument docEmptyFile = new XmlDocument();
			docMyFile.Load(inputSvgFilePath);
			docEmptyFile.Load(templateSvgFilePath);

			XmlNodeList pathNodeList = docMyFile.GetElementsByTagName("filePath");

			// For each path
			for (int i = 0; i < pathNodeList.Count; i++)
			{
				XmlElement newLayerElement = docEmptyFile.CreateElement("g");
				newLayerElement.SetAttribute("__groupmode", "layer");
				newLayerElement.SetAttribute("id", (i + 1).ToString());
				newLayerElement.SetAttribute("__label", (i + 1).ToString());
				newLayerElement.SetAttribute("style", "display:inline"); // display:inline for visible, display:none for hidden

				// get all the path nodes as strings and put into fragment and add fragment to new element
				XmlDocumentFragment fragment = docEmptyFile.CreateDocumentFragment();
				fragment.InnerXml = pathNodeList[i].OuterXml;
				newLayerElement.AppendChild(fragment);

				// add the newLayerElement to the empty xml file
				docEmptyFile.DocumentElement.AppendChild(newLayerElement);
			}

			// Clean up new svg file
			string innerXml = docEmptyFile.InnerXml.Replace("__groupmode", "inkscape:groupmode");
			innerXml = innerXml.Replace("__label", "inkscape:label");
			innerXml = innerXml.Replace("xmlns=\"\"", "");

			// Save file
			using (StreamWriter writer = new StreamWriter(outputSvgFilePath))
			{
				writer.Write(innerXml);
			}
		}


		// Get Text from inscape file
		public static void GetTextFromInkscapeFile(string svgFilePath, string filePathOutput)
		{
			ArgumentUtil.IsFilePath(svgFilePath, "svgFilePath", "GetTextFromInkscapeFile");
			ArgumentUtil.IsNotWhiteSpaceOrNull(filePathOutput, "filePathOutput", "GetTextFromInkscapeFile");		

			XmlDocument doc = new XmlDocument();
			doc.Load(svgFilePath);
			XmlNodeList nodeList = doc.GetElementsByTagName("text");

			using (StreamWriter writer = new StreamWriter(filePathOutput))
			{
				foreach (XmlNode node in nodeList)
					writer.WriteLine(node.InnerText);
			}
		}


		#region Export from Inkscape to Image

		// Exports an Svg file to an Image using Inkscape to do the conversion
		public static void ExportSvgToImage(string svgFilePath)
		{
			ArgumentUtil.IsFilePath(svgFilePath, "svgFilePath", "ExportSvgToImage");

			string filePath = @"C:\Program Files (x86)\Inkscape\";
			string fileName = "inkscape.exe";

			// Check inkscape.exe exists
			if (File.Exists(filePath + fileName) == false)
			{
				MessageBox.Show("Check you have Inkscape installed here : " + filePath + fileName);
				return;
			}
			
			ProcessStartInfo info = new ProcessStartInfo();
			info.WorkingDirectory = filePath;
			info.FileName = fileName;
			info.Arguments = svgFilePath;

			Process.Start(info);
		}


		// TODO : does this work ??
		// Saves all layers to their own png file
		public static string SaveLayersAsImages(string inkscapeFilePath, string outputFolderPath)
		{
			ArgumentUtil.IsFilePath(inkscapeFilePath, "inkscapeFilePath", "SaveLayersAsImages");
			ArgumentUtil.IsFolderPath(outputFolderPath, "outputFolderPath", "SaveLayersAsImages");

			string inkscape = @"C:\Program Files (x86)\Inkscape\inkscape.com";					
			string command = @"/C " + inkscape + " -f " + inkscapeFilePath + " -e " + outputFolderPath + "test.png";

			Process.Start("cmd.exe", command);
			return "Images Created !";
		}

		#endregion
		
	}
}
