using System;

namespace Verse
{
	// Token: 0x02000B13 RID: 2835
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x040027F8 RID: 10232
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x040027F9 RID: 10233
		public int damagePerTickRare = 1;

		// Token: 0x06003EA4 RID: 16036 RVA: 0x0020FD43 File Offset: 0x0020E143
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}
	}
}
