using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Common
{

	// TODO : test this class manually or work out some other way
	public class ImageUtil
	{

		#region Load Image

		// Returns a BitmapImage from a filepath
		public static BitmapImage LoadBitmapImageFromFile(string filePath)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "LoadBitmapImageFromFile");

			BitmapImage image = new BitmapImage();
			image.BeginInit();
			image.CacheOption = BitmapCacheOption.OnLoad;  // this does a thing
			image.UriSource = new Uri(filePath);
			image.Freeze();  // stops something bad happening
			image.EndInit();
			return image;
		}


		// Get a BitmapImage from the Resources folder
		public static BitmapImage LoadBitmapImageFromResources(string imagePath)
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(imagePath, "imagePath", "LoadBitmapImageFromResources");

			BitmapImage bmp = new BitmapImage();
			bmp.BeginInit();
			bmp.UriSource = new Uri(@"pack://application:,,,/" + imagePath, UriKind.RelativeOrAbsolute);
			bmp.EndInit();
			return bmp;
		}


		// Returns the windows explorer icon of the file as an imageSource
		public static ImageSource GetFileIcon(string filePath)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "GetFileIcon");

			Icon icon = Icon.ExtractAssociatedIcon(filePath);
			if (icon != null)
			{
				Image image = icon.ToBitmap() as Image;
				if (image != null)
					return Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
						new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
				else
					return null;
			}
			else
				return null;
		}

		#endregion


		#region Save Image

		// Save a BitmapSource to a specified filepath as a PNG image
		public static void SaveBitmapImageToFile(BitmapImage image, string filePath)
		{
			ArgumentUtil.IsNotNull(image, "image", "SaveBitmapImageToFile");
			ArgumentUtil.IsFilePath(filePath, "filePath", "SaveBitmapImageToFile");

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				BitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(image));
				encoder.Save(fileStream);
			}
		}

		#endregion


		#region Convert Image

		// Convert BitmapSource to BitmapImage
		//http://stackoverflow.com/questions/5338253/bitmapsource-to-bitmapimage
		public static BitmapImage ConvertBitmapSourceToBitmapImage(BitmapSource source)
		{
			ArgumentUtil.IsNotNull(source, "source", "ConvertBitmapSourceToBitmapImage");

			BitmapImage image = null;
			JpegBitmapEncoder encoder = new JpegBitmapEncoder();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				image = new BitmapImage();
				encoder.Frames.Add(BitmapFrame.Create(source));
				encoder.Save(memoryStream);
				image.BeginInit();
				image.StreamSource = new MemoryStream(memoryStream.ToArray());
				image.EndInit();
			}

			return image;
		}


		// Convert BitmapImage to byte[]
		//http://stackoverflow.com/questions/6597676/bitmapimage-to-byte
		public static byte[] ConvertBitmapImageToByteArray(BitmapImage image)
		{
			ArgumentUtil.IsNotNull(image, "image", "ConvertBitmapImageToByteArray");

			byte[] data;
			using (MemoryStream ms = new MemoryStream())
			{
				PngBitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(image));
				encoder.Save(ms);
				data = ms.ToArray();
			}

			return data;
		}


		// TODO : fix up, what does the Ogg Video File thing do - high class coupling value too
		// Make an image from a string		
		// http://msdn.microsoft.com/en-us/library/system.windows.media.imaging.rendertargetbitmap(v=vs.110).aspx
		public static ImageSource ConvertTextToImageSource(string text)
		{
			if (text == null)
				return null;

			FormattedText formattedText = new FormattedText("Ogg Video File", new CultureInfo("en-us"), FlowDirection.LeftToRight,
					new Typeface("Arial"), 22, System.Windows.Media.Brushes.Black);
			formattedText.TextAlignment = TextAlignment.Center;

			FormattedText formattedText2 = new FormattedText(text, new CultureInfo("en-us"), FlowDirection.LeftToRight,
					new Typeface("Arial"), 18, System.Windows.Media.Brushes.Black);
			formattedText.TextAlignment = TextAlignment.Center;

			DrawingVisual drawingVisual = new DrawingVisual();
			using (DrawingContext drawingContext = drawingVisual.RenderOpen())
			{
				drawingContext.DrawRectangle(System.Windows.Media.Brushes.White, null, new Rect(0, 0, 180, 180));
				drawingContext.DrawText(formattedText, new System.Windows.Point(90, 5));
				drawingContext.DrawText(formattedText2, new System.Windows.Point(5, 80));
			}

			RenderTargetBitmap bmp = new RenderTargetBitmap(180, 180, 90, 90, PixelFormats.Pbgra32);
			bmp.Render(drawingVisual);

			return bmp;
		}

		#endregion
		
	}
}
