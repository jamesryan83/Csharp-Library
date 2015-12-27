using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using CommonUI;

namespace CommonUI.Behaviors
{

	/* How to use...
		
		add rectangles like this to the outer edge of of the window to resize...	  	  
	    <Rectangle Name="RectangleResizeNW" Grid.Row="0" Grid.Column="0" Width="5" Height="5" Fill="Transparent" Tag="ResizingRectangle" />
		<Rectangle Name="RectangleResizeN" Grid.Row="0" Grid.Column="1" Height="5" Fill="Transparent" Tag="ResizingRectangle" />
		<Rectangle Name="RectangleResizeNE" Grid.Row="0" Grid.Column="2" Width="5" Height="5" Fill="Transparent" Tag="ResizingRectangle"/>

		<Rectangle Name="RectangleResizeW" Grid.Row="1" Grid.Column="0" Width="5" Fill="Transparent" Tag="ResizingRectangle" />
		<Rectangle Name="RectangleResizeE" Grid.Row="1" Grid.Column="2" Width="5" Fill="Transparent" Tag="ResizingRectangle" />

		<Rectangle Name="RectangleResizeSW" Grid.Row="2" Grid.Column="0" Width="5" Height="5" Fill="Transparent"  Tag="ResizingRectangle"/>
		<Rectangle Name="RectangleResizeS" Grid.Row="2" Grid.Column="1" Height="5" Fill="Transparent" Tag="ResizingRectangle" />
		<Rectangle Name="RectangleResizeSE" Grid.Row="2" Grid.Column="2" Width="5" Height="5" Fill="Transparent" Tag="ResizingRectangle" />
	
		And then put this in the mainwindow codebehind...		
	 
		WindowResizing windowResizing;
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			windowResizing = new WindowResizing(this);
		}
		
	 
	 */

	// For resizing mainwindow
	public enum ResizeDirection
	{
		Left = 1,
		Right = 2,
		Top = 3,
		TopLeft = 4,
		TopRight = 5,
		Bottom = 6,
		BottomLeft = 7,
		BottomRight = 8,
	}


	// For the resizing rectangles around the outside of the window
	// this uses SendMessage from user32.dll to update the window size with system stuff
	// The code here is setting up the mouse events and showing the different mouse cursors
	// this is based on this article http://goo.gl/gHi2BG
	public class WindowResizing
	{
		private Window window;

		// Uses user32.dll to resize window
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
		private HwndSource hwndSource;


		// Constructor
		public WindowResizing(Window window)
		{
			this.window = window;

			// Get a list of all the resizing rectangles in the window
			List<Rectangle> resizingRectangles = new List<Rectangle>();
			WindowUtil.FindAllControlsWithTag<Rectangle>(window, "ResizingRectangle", ref resizingRectangles);

			// Add Events to resizing rectanges
			for (int i = 0; i < resizingRectangles.Count; i++)
			{
				resizingRectangles[i].MouseEnter += RectangleResize_MouseEnter;
				resizingRectangles[i].MouseLeave += RectangleResize_MouseLeave;
				resizingRectangles[i].MouseDown += RectangleResize_MouseDown;
			}


			// Put this window into the hwndSource so it can use SendMessage from user32.dll
			hwndSource = PresentationSource.FromVisual((Visual) window) as HwndSource;
			hwndSource.AddHook(new HwndSourceHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) => { return IntPtr.Zero; }));
		}


		// Message to Resize Window - SendMessage does all the resizing logic of the window
		private void ResizeWindow(ResizeDirection direction)
		{
			SendMessage(hwndSource.Handle, 0x112, (IntPtr) (61440 + direction), IntPtr.Zero);
		}


		// Resizing Rectangle Cursor and Resizing
		public void ShowResizeHandle(string resizeRectangleName, bool resize)
		{
			switch (resizeRectangleName)
			{
				case "RectangleResizeN":
					window.Cursor = Cursors.SizeNS;
					if (resize == true) ResizeWindow(ResizeDirection.Top);
					break;
				case "RectangleResizeS":
					window.Cursor = Cursors.SizeNS;
					if (resize == true) ResizeWindow(ResizeDirection.Bottom);
					break;
				case "RectangleResizeW":
					window.Cursor = Cursors.SizeWE;
					if (resize == true) ResizeWindow(ResizeDirection.Left);
					break;
				case "RectangleResizeE":
					window.Cursor = Cursors.SizeWE;
					if (resize == true) ResizeWindow(ResizeDirection.Right);
					break;
				case "RectangleResizeNW":
					window.Cursor = Cursors.SizeNWSE;
					if (resize == true) ResizeWindow(ResizeDirection.TopLeft);
					break;
				case "RectangleResizeNE":
					window.Cursor = Cursors.SizeNESW;
					if (resize == true) ResizeWindow(ResizeDirection.TopRight);
					break;
				case "RectangleResizeSW":
					window.Cursor = Cursors.SizeNESW;
					if (resize == true) ResizeWindow(ResizeDirection.BottomLeft);
					break;
				case "RectangleResizeSE":
					window.Cursor = Cursors.SizeNWSE;
					if (resize == true) ResizeWindow(ResizeDirection.BottomRight);
					break;
				default:
					break;
			}
		}


		// Resizing Rectangle Mouse Enter - show cursor
		private void RectangleResize_MouseEnter(object sender, MouseEventArgs e)
		{
			System.Windows.Shapes.Rectangle clickedRectangle = sender as System.Windows.Shapes.Rectangle;
			if (clickedRectangle != null)
				ShowResizeHandle(clickedRectangle.Name, false);
		}


		// Resizing Rectangle Mouse Leave - return to default cursor
		private void RectangleResize_MouseLeave(object sender, MouseEventArgs e)
		{
			if (Mouse.LeftButton != MouseButtonState.Pressed)
				window.Cursor = Cursors.Arrow;
		}


		// Resizing Rectangle Mouse Down - show cursor and resize window
		private void RectangleResize_MouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			System.Windows.Shapes.Rectangle clickedRectangle = sender as System.Windows.Shapes.Rectangle;
			if (clickedRectangle != null)
				ShowResizeHandle(clickedRectangle.Name, true);
		}
	}
}
