using System;

namespace Verse
{
	// Token: 0x02000FA6 RID: 4006
	public static class TemperatureDisplayModeExtension
	{
		// Token: 0x060060F8 RID: 24824 RVA: 0x0031038C File Offset: 0x0030E78C
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
