using System;
using Verse;

namespace RimWorld
{
	public class CompMelter : ThingComp
	{
		private const float MeltPerIntervalPer10Degrees = 0.15f;

		public CompMelter()
		{
		}

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
