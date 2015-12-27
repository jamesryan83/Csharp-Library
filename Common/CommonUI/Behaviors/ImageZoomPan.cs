using System.Windows;
using System.Windows.Media;

namespace CommonUI.Behaviors
{
	// TODO : test this class
	class ImageZoomPan
	{
		/* How to use....
			this goes in the xaml for the window
		 
		    <Grid Name="GridOuter" MouseRightButtonDown="GridImages_MouseRightButtonDown" 
				MouseLeftButtonDown="GridImages_MouseLeftButtonDown" MouseMove="GridImages_MouseMove" 
					MouseLeftButtonUp="GridImages_MouseLeftButtonUp" MouseWheel="GridImages_MouseWheel" ClipToBounds="True">
			
				<Grid Name="GridImages" />
			</Grid>		 
		*/

		private Point current, start;
		Matrix originalPosition;
		FrameworkElement gridOuter, gridImages;


		// Constructor
		public ImageZoomPan(FrameworkElement gridOuter, FrameworkElement gridImages)
		{
			this.gridOuter = gridOuter;
			this.gridImages = gridImages;
			this.originalPosition = gridImages.RenderTransform.Value;
		}


		// Start moving image
		private void GridImages_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (gridOuter.IsMouseCaptured)
				return;

			gridOuter.CaptureMouse();

			start = e.GetPosition(gridOuter);
			current.X = gridImages.RenderTransform.Value.OffsetX;
			current.Y = gridImages.RenderTransform.Value.OffsetY;
		}


		// Move image
		private void GridImages_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (gridOuter.IsMouseCaptured == false)
				return;

			Point p = e.MouseDevice.GetPosition(gridOuter);

			Matrix m = gridImages.RenderTransform.Value;

			m.OffsetX = current.X + (p.X - start.X); // need to check this
			m.OffsetY = current.Y + (p.Y - start.Y);

			gridImages.RenderTransform = new MatrixTransform(m);
		}


		// Stop moving image
		private void GridImages_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (gridOuter.IsMouseCaptured == true)
				gridOuter.ReleaseMouseCapture();
		}


		// Zoom image
		private void GridImages_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
		{
			Point p = e.MouseDevice.GetPosition(gridImages);

			Matrix m = gridImages.RenderTransform.Value;

			if (e.Delta > 0)
				m.ScaleAtPrepend(1.1, 1.1, p.X, p.Y);
			else
				m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, p.X, p.Y);

			gridImages.RenderTransform = new MatrixTransform(m);
		}


		// Reset image to original location/zoom
		private void resetImagePosition()
		{
			originalPosition.ScaleAtPrepend(1, 1, gridOuter.ActualWidth / 2, gridOuter.ActualHeight / 2);
			gridImages.RenderTransform = new MatrixTransform(originalPosition);
		}
	}
}
