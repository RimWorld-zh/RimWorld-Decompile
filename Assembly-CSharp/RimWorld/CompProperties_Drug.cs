using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompProperties_Drug : CompProperties
	{
		public ChemicalDef chemical = null;

		public float addictiveness = 0f;

		public float minToleranceToAddict = 0f;

		public float existingAddictionSeverityOffset = 0.1f;

		public float needLevelOffset = 1f;

		public FloatRange overdoseSeverityOffset = FloatRange.Zero;

		public float largeOverdoseChance = 0f;

		public bool isCombatEnhancingDrug = false;

		public float listOrder = 0f;

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
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0(parentDef).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.Addictive)
				yield break;
			if (this.chemical != null)
				yield break;
			yield return "addictive but chemical is null";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0106:
			/*Error near IL_0107: Unexpected return in MoveNext()*/;
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			using (IEnumerator<StatDrawEntry> enumerator = this._003CSpecialDisplayStats_003E__BaseCallProxy1().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					StatDrawEntry s = enumerator.Current;
					yield return s;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.Addictive)
				yield break;
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Addictiveness".Translate(), this.addictiveness.ToStringPercent(), 0, "");
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0117:
			/*Error near IL_0118: Unexpected return in MoveNext()*/;
		}
	}
}
