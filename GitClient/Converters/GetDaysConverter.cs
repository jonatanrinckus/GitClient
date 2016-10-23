using System;
using System.Globalization;
using System.Windows.Data;

namespace GitClient.Converters
{
	public class GetDaysConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var time = (DateTime.UtcNow - (DateTimeOffset)value);

			return time.Days;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
