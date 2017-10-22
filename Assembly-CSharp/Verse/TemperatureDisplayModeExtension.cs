using System;

namespace Verse
{
	public static class TemperatureDisplayModeExtension
	{
		public static string ToStringHuman(this TemperatureDisplayMode mode)
		{
			string result;
			switch (mode)
			{
			case TemperatureDisplayMode.Celsius:
			{
				result = "Celsius".Translate();
				break;
			}
			case TemperatureDisplayMode.Fahrenheit:
			{
				result = "Fahrenheit".Translate();
				break;
			}
			case TemperatureDisplayMode.Kelvin:
			{
				result = "Kelvin".Translate();
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
			}
			return result;
		}
	}
}
