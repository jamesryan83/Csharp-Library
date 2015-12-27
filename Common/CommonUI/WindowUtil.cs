using Common;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CommonUI
{
	// Methods for use with Xaml
	public class WindowUtil
	{
		// Finds all ui controls with the a certan tag and adds them to the elements list
		public static void FindAllControlsWithTag<T>(DependencyObject parent, string tag, ref List<T> elementList) where T : UIElement
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);

				if (child != null)
				{
					if (typeof(FrameworkElement).IsAssignableFrom(child.GetType()) && 
						((string) ((FrameworkElement) child).Tag == tag))
						elementList.Add(child as T);

					FindAllControlsWithTag<T>(child, tag, ref elementList);
				}
			}
		}


		// Returns a parent from a child by type
		public static T FindParent<T>(DependencyObject child) where T : DependencyObject
		{
			//get parent item
			DependencyObject parentObject = VisualTreeHelper.GetParent(child);

			//we've reached the end of the tree
			if (parentObject == null) return null;

			//check if the parent matches the type we're looking for
			T parent = parentObject as T;
			if (parent != null)
				return parent;
			else
				return FindParent<T>(parentObject);
		}


		// Returns a string array from a drop event (e.g. drop folder/file onto a textbox)
		public static string[] GetDataFromDropEvent(DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				return ((string[]) e.Data.GetData(DataFormats.FileDrop));
			else
				return null;
		}


		// Returns a BitmapImage from a Canvas
		public static BitmapImage GetBitmapImageFromCanvas(InkCanvas canvas)
		{
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) canvas.ActualWidth,
				(int) canvas.ActualHeight, 96d, 96d, PixelFormats.Default);

			renderTargetBitmap.Render(canvas);

			return ImageUtil.ConvertBitmapSourceToBitmapImage(renderTargetBitmap);
		}
	}
}
