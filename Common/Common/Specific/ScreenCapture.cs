using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Common.Specific
{

	// http://10rem.net/blog/2011/02/08/capturing-screen-images-in-wpf-using-gdi-win32-and-a-little-wpf-interop-help
	public class ScreenCapture
	{

		// Returns a BitmapSource from an area of the Screen
		public static BitmapSource CaptureScreen(Rectangle screenAreaRectangle)
		{
			ArgumentUtil.IsNotNull<Rectangle>(screenAreaRectangle, "screenAreaRectangle", "CaptureScreen", "Error capturing screen");

			Point topLeft, bottomRight;
			topLeft = screenAreaRectangle.PointToScreen(new Point(0, 0));
			bottomRight = screenAreaRectangle.PointToScreen(new Point(
				screenAreaRectangle.ActualWidth, screenAreaRectangle.ActualHeight));

			return CaptureRegion
			(
				GetDesktopWindow(),
				Convert.ToInt32(topLeft.X),
				Convert.ToInt32(topLeft.Y),
				Convert.ToInt32(bottomRight.X - topLeft.X),
				Convert.ToInt32(bottomRight.Y - topLeft.Y)
			);
		}


		private const int SRCCOPY = 0xCC0020;

		[DllImport("user32.dll")]
		private static extern IntPtr GetDesktopWindow();

		// http://msdn.microsoft.com/en-us/library/dd144871(VS.85).aspx
		[DllImport("user32.dll")]
		private static extern IntPtr GetDC(IntPtr hwnd);

		// http://msdn.microsoft.com/en-us/library/dd162920(VS.85).aspx
		[DllImport("user32.dll")]
		private static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

		// http://msdn.microsoft.com/en-us/library/dd183370(VS.85).aspx
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, Int32 dwRop);

		// http://msdn.microsoft.com/en-us/library/dd183488(VS.85).aspx
		[DllImport("gdi32.dll")]
		private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

		// http://msdn.microsoft.com/en-us/library/dd183489(VS.85).aspx
		[DllImport("gdi32.dll", SetLastError = true)]
		private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		// http://msdn.microsoft.com/en-us/library/dd162957(VS.85).aspx
		[DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
		private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		// http://msdn.microsoft.com/en-us/library/dd183539(VS.85).aspx
		[DllImport("gdi32.dll")]
		private static extern bool DeleteObject(IntPtr hObject);


		// Convert the selected screen region into a BitmapSource
		// http://10rem.net/blog/2011/02/08/capturing-screen-images-in-wpf-using-gdi-win32-and-a-little-wpf-interop-help
		private static BitmapSource CaptureRegion(IntPtr hWnd, int x, int y, int width, int height)
		{
			IntPtr sourceDC = IntPtr.Zero;
			IntPtr targetDC = IntPtr.Zero;
			IntPtr compatibleBitmapHandle = IntPtr.Zero;
			BitmapSource bitmapSource = null;

			try
			{
				// gets the main desktop and all open windows
				sourceDC = GetDC(GetDesktopWindow());
				//sourceDC = User32.GetDC(hWnd);
				targetDC = CreateCompatibleDC(sourceDC);
				// create a bitmap compatible with our target DC
				compatibleBitmapHandle = CreateCompatibleBitmap(sourceDC, width, height);
				// gets the bitmap into the target device context
				SelectObject(targetDC, compatibleBitmapHandle);
				// copy from source to destination
				BitBlt(targetDC, 0, 0, width, height, sourceDC, x, y, SRCCOPY);
				// It converts from an hBitmap to a BitmapSource.				
				bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
					compatibleBitmapHandle, IntPtr.Zero, Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());

				return bitmapSource;
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show("Error creating BitmapSource\n\n" + e.Message + "\n\n" + e.StackTrace);
				return null;
			}
			finally
			{
				DeleteObject(compatibleBitmapHandle);

				ReleaseDC(IntPtr.Zero, sourceDC);
				ReleaseDC(IntPtr.Zero, targetDC);
			}
		}
		
	}
}
