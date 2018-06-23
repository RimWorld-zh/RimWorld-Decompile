using System;

namespace Verse
{
	// Token: 0x02000B10 RID: 2832
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x040027F0 RID: 10224
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x040027F1 RID: 10225
		public int damagePerTickRare = 1;

		// Token: 0x06003EA0 RID: 16032 RVA: 0x0020F937 File Offset: 0x0020DD37
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}
	}
}
