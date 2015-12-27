using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CommonUI
{
	class AnimationUtil
	{
		// TODO : this probably shouldn't be getting created each time this is run
		// Runs a scale animation on a ui element
		public static void StartScaleAnimation(FrameworkElement element)
		{
			// Create a storyboard to add the animations to and set the centre of the ui element
			Storyboard storyboard = new Storyboard();
			ScaleTransform scale = new ScaleTransform(1.0, 1.0);
			element.RenderTransformOrigin = new Point(0.5, 0.5);
			element.RenderTransform = scale;

			// bounce type scale animation requires 4 animations. 2 for zoom in x,y, and 2 for zoom out x,y
			AddScaleAnimation(element, storyboard, "RenderTransform.ScaleX", 100, 1, 0.8);
			AddScaleAnimation(element, storyboard, "RenderTransform.ScaleY", 100, 1, 0.8);
			AddScaleAnimation(element, storyboard, "RenderTransform.ScaleX", 100, 0.8, 1, 100);
			AddScaleAnimation(element, storyboard, "RenderTransform.ScaleY", 100, 0.8, 1, 100);

			storyboard.Begin();
		}

		// Adds a single scale animation to a storyboard for a ui control
		public static void AddScaleAnimation(FrameworkElement control, Storyboard storyboard, string propertyPath, 
			double duration, double from, double to, double beginTime = 0)
		{
			DoubleAnimation scaleAnimation = new DoubleAnimation
			{
				Duration = TimeSpan.FromMilliseconds(duration),
				From = from,
				To = to,
				BeginTime = TimeSpan.FromMilliseconds(beginTime)
			};

			storyboard.Children.Add(scaleAnimation);
			Storyboard.SetTargetProperty(scaleAnimation, new PropertyPath(propertyPath));
			Storyboard.SetTarget(scaleAnimation, control);
		}
	}
}
