using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000721 RID: 1825
	public class CompMelter : ThingComp
	{
		// Token: 0x040015FF RID: 5631
		private const float MeltPerIntervalPer10Degrees = 0.15f;

		// Token: 0x06002836 RID: 10294 RVA: 0x00157E40 File Offset: 0x00156240
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
