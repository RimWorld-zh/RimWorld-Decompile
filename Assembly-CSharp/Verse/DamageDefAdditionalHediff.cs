using System;
using RimWorld;

namespace Verse
{
	public class DamageDefAdditionalHediff
	{
		public HediffDef hediff = null;

		public float severityPerDamageDealt = 0.1f;

		public StatDef victimSeverityScaling = null;

		public DamageDefAdditionalHediff()
		{
		}
	}
}
