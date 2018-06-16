using System;

namespace Verse
{
	// Token: 0x02000FA7 RID: 4007
	public static class TemperatureDisplayModeExtension
	{
		// Token: 0x060060D1 RID: 24785 RVA: 0x0030E20C File Offset: 0x0030C60C
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
