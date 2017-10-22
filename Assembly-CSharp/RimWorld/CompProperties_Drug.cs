using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompProperties_Drug : CompProperties
	{
		public ChemicalDef chemical;

		public float addictiveness;

		public float minToleranceToAddict;

		public float existingAddictionSeverityOffset = 0.1f;

		public float needLevelOffset = 1f;

		public FloatRange overdoseSeverityOffset = FloatRange.Zero;

		public float largeOverdoseChance;

		public bool isCombatEnhancingDrug;

		public float listOrder;

		public bool Addictive
		{
			get
			{
				return this.addictiveness > 0.0;
			}
		}

		public bool CanCauseOverdose
		{
			get
			{
				return this.overdoseSeverityOffset.TrueMax > 0.0;
			}
		}

		public CompProperties_Drug()
		{
			base.compClass = typeof(CompDrug);
		}

		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string item in base.ConfigErrors(parentDef))
			{
				yield return item;
			}
			if (this.Addictive && this.chemical == null)
			{
				yield return "addictive but chemical is null";
			}
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry item in base.SpecialDisplayStats())
			{
				yield return item;
			}
			if (this.Addictive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Addictiveness".Translate(), this.addictiveness.ToStringPercent(), 0);
			}
		}
	}
}
