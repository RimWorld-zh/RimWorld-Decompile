using System;

namespace Verse
{
	// Token: 0x02000FAA RID: 4010
	public static class TemperatureDisplayModeExtension
	{
		// Token: 0x06006102 RID: 24834 RVA: 0x00310A0C File Offset: 0x0030EE0C
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
