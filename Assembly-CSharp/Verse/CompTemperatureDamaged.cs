using RimWorld;

namespace Verse
{
	public class CompTemperatureDamaged : ThingComp
	{
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)base.props;
			}
		}

		public override void CompTick()
		{
			Log.ErrorOnce("This thingcomp only supports TickRare tickerType", 2138971);
		}

		public override void CompTickRare()
		{
			if (!this.Props.safeTemperatureRange.Includes(base.parent.AmbientTemperature))
			{
				base.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, this.Props.damagePerTickRare, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}
		}
	}
}
