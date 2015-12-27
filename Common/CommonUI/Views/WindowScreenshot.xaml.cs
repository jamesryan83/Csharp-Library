using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CommonUI.Views
{    

	// TODO : test this works
    public partial class WindowScreenshot : Window
    {
        private double xPosStart, yPosStart;		
		private bool isMouseDown = false;
		private Rectangle rect;
		public BitmapSource bitmapSourceImage;
		

		public WindowScreenshot()
        {
            InitializeComponent();

			List<int[]> screenSizes = Common.SystemUtil.GetScreenSizes();

			// Set size of window to go across multiple screens
			int width = 0;
			int height = 0;
			foreach (int[] screens in screenSizes)
			{
				width += screens[0];
				height = screens[1] > height ? screens[1] : height; // use biggest height
			}

			ScreenshotWindow.Left = 0;
			ScreenshotWindow.Top = 0;
			ScreenshotWindow.Width = width;
			ScreenshotWindow.Height = height;
        }


		#region Mouse Events

		// starts the selection window
		private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
			xPosStart = e.GetPosition(null).X;
			yPosStart = e.GetPosition(null).Y;
			
			rect = new Rectangle();
			rect.Fill = new SolidColorBrush(Colors.White);
			
			canvasCapture.Children.Add(rect);
			Canvas.SetLeft(rect, xPosStart);
			Canvas.SetTop(rect, yPosStart);
        }


		// adjusts the selection window as the mouse is moved
		private void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isMouseDown == true)
            {  
				double newX = e.GetPosition(null).X;
 				double newY = e.GetPosition(null).Y;

				if (newX > xPosStart)
					rect.Width = newX - xPosStart;
				else if (newX < xPosStart)
				{
					rect.Width = xPosStart - newX;
					Canvas.SetLeft(rect, newX);
				}

				if (newY > yPosStart)
					rect.Height = newY - yPosStart;
				else if (newY < yPosStart)
				{
					rect.Height = yPosStart - newY;
					Canvas.SetTop(rect, newY);
				}

				// This is how you clip a hole in another shape, but I couldn't get it to work properly...
				//RectangleGeometry bounds = new RectangleGeometry(new Rect(0, 0, ScreenshotWindow.Width, ScreenshotWindow.Height));
				//RectangleGeometry hole = new RectangleGeometry(new Rect(xPosStart, yPosStart, xPosFinish, yPosFinish));								
				//Geometry combined = Geometry.Combine(bounds, hole, GeometryCombineMode.Exclude, null);
				//ScreenshotWindow.Clip = combined;
            }
        }

		// When the mouse is released, create the screenshot and close the window
		private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			isMouseDown = false;

			bitmapSourceImage = Common.Specific.ScreenCapture.CaptureScreen(rect);
			
			this.Close();
		}

		#endregion


		#region Other Methods

		private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			// Close window when escape is pressed
			if (e.Key == Key.Escape)
			{
				this.Close();
			}
		}

		#endregion

    }
}
