using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071F RID: 1823
	public class CompMelter : ThingComp
	{
		// Token: 0x040015FB RID: 5627
		private const float MeltPerIntervalPer10Degrees = 0.15f;

		// Token: 0x06002833 RID: 10291 RVA: 0x00157A90 File Offset: 0x00155E90
		public override void CompTickRare()
		{
			float ambientTemperature = this.parent.AmbientTemperature;
			if (ambientTemperature >= 0f)
			{
				float f = 0.15f * (ambientTemperature / 10f);
				int num = GenMath.RoundRandom(f);
				if (num > 0)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			}
		}
	}
}
