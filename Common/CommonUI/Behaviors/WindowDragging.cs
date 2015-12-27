using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CommonUI.Behaviors
{
	// To use add
	// windowDragging = new WindowDragging(this);
	// to constructor of window, where this is the window instance

	public class WindowDragging
	{
		public Window window;

		// some variables for dragging window around
		private bool dragging = false;
		private Point point;
		private bool isCaptured = false;
		public bool minimizeable = false;


		// Constructor
		public WindowDragging(Window window, bool minimizeable)
		{
			this.window = window;
			this.minimizeable = minimizeable;

			window.MouseDown += WindowDragging_MouseDown;
			window.MouseMove += WindowDragging_MouseMove;
			window.MouseUp += WindowDragging_MouseUp;
			window.Activated += WindowDragging_Activated;
		}


		// Main window mouse down
		public void WindowDragging_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// minimise on double click
			if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2 && minimizeable == true)
			{
				// turn on single border window so minimise animation happens.  Turn border off again in WindowMain_Activated()
				window.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
				window.WindowState = System.Windows.WindowState.Minimized;
				return;
			}

			// otherwise start dragging
			point = window.PointToScreen(e.GetPosition(window));
			dragging = true;
		}


		// Main window mouse move
		public void WindowDragging_MouseMove(object sender, MouseEventArgs e)
		{
			if (dragging == true)
			{
				if (isCaptured == false)
				{
					window.CaptureMouse();
					isCaptured = true;
				}
				Point currentPoint = window.PointToScreen(e.GetPosition(window));
				window.Left = window.Left + currentPoint.X - point.X;
				window.Top = window.Top + currentPoint.Y - point.Y;
				point = currentPoint;
			}
		}


		// Main window mouse up
		public void WindowDragging_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (dragging == true)
			{
				dragging = false;
				window.ReleaseMouseCapture();
				isCaptured = false;
			}
		}

		
		// Remove single border window style - required for maximise animation		
		// https://goo.gl/riRCTV
		public void WindowDragging_Activated(object sender, EventArgs e)
		{	
			if (window.WindowStyle != WindowStyle.None)
			{
				window.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (DispatcherOperationCallback) delegate(object unused)
				{
					window.WindowStyle = WindowStyle.None;
					return null;
				}, null);
			}
		}
	}
}
