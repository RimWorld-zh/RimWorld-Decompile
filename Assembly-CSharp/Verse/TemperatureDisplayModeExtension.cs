using System;

namespace Verse
{
	// Token: 0x02000FA6 RID: 4006
	public static class TemperatureDisplayModeExtension
	{
		// Token: 0x060060CF RID: 24783 RVA: 0x0030E2E8 File Offset: 0x0030C6E8
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
