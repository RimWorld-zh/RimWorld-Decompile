using System;

namespace Verse
{
	// Token: 0x02000B12 RID: 2834
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x040027F1 RID: 10225
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x040027F2 RID: 10226
		public int damagePerTickRare = 1;

		// Token: 0x06003EA4 RID: 16036 RVA: 0x0020FA63 File Offset: 0x0020DE63
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}
	}
}
