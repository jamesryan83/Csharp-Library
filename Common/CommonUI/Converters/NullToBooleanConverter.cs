using System;
using System.Windows.Data;

namespace CommonUI.Converters
{
	// How to use...
	/* 
		Add this to window...
		xmlns:converters="clr-namespace:CommonUI.Converters"
	 
		<Window.Resources>		
			<converters:NullToBooleanConverter x:Key="NullToBooleanConverter"/>
		...
		</Window.Resources>		
	 
		and to use it on an element...
		Visibility="{Binding Converter={x:Static converters:NullToBooleanConverter.Instance}, Path=propertyToCheckForNull}"
	*/


	// Converts null to boolean
	public class NullToBooleanConverter : IValueConverter
	{
		public static NullToBooleanConverter Instance = new NullToBooleanConverter();

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value == null ? true : false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is bool)
			{
				bool temp = (bool) value;
				return temp == true ? null : value;
			}
			else
				return true;
		}
	}
}
