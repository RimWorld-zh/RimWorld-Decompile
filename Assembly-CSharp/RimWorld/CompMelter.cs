using Verse;

namespace RimWorld
{
	public class CompMelter : ThingComp
	{
		private const float MeltPerIntervalPer10Degrees = 0.15f;

		public override void CompTickRare()
		{
			float ambientTemperature = base.parent.AmbientTemperature;
			if (!(ambientTemperature < 0.0))
			{
				float f = (float)(0.15000000596046448 * (ambientTemperature / 10.0));
				int num = GenMath.RoundRandom(f);
				if (num > 0)
				{
					base.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, num, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}
			}
		}
	}
}
