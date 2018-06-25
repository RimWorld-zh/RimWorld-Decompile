using System;

namespace Verse
{
	public static class TemperatureDisplayModeExtension
	{
		public static string ToStringHuman(this TemperatureDisplayMode mode)
		{
			string result;
			if (mode != TemperatureDisplayMode.Celsius)
			{
				if (mode != TemperatureDisplayMode.Fahrenheit)
				{
					if (mode != TemperatureDisplayMode.Kelvin)
					{
						throw new NotImplementedException();
					}
					result = "Kelvin".Translate();
				}
				else
				{
					result = "Fahrenheit".Translate();
				}
			}
			else
			{
				result = "Celsius".Translate();
			}
			return result;
		}
	}
}
