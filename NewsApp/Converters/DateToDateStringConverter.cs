using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewsApp.Converters
{
	public class DateToDateStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
			{
				DateTime dateValue = (DateTime)value;
				return $"{dateValue.Date.DayOfWeek.ToString()}, {dateValue.Date.Day.ToString()}, {dateValue.ToString("MMMM")}, {dateValue.Date.Year}";
			}
			else
				return string.Empty;
			
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.Empty;
		}
	}
}