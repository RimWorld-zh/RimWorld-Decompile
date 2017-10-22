using RimWorld;
using System.Collections.Generic;

namespace Verse
{
	public class HediffStage
	{
		public float minSeverity = 0f;

		public string label = (string)null;

		public bool becomeVisible = true;

		public bool lifeThreatening = false;

		public TaleDef tale = null;

		public float vomitMtbDays = -1f;

		public float deathMtbDays = -1f;

		public float painFactor = 1f;

		public float painOffset = 0f;

		public float forgetMemoryThoughtMtbDays = -1f;

		public float pctConditionalThoughtsNullified = 0f;

		public float opinionOfOthersFactor = 1f;

		public float hungerRateFactor = 1f;

		public float hungerRateFactorOffset = 0f;

		public float restFallFactor = 1f;

		public float restFallFactorOffset = 0f;

		public float socialFightChanceFactor = 1f;

		public float mentalBreakMtbDays = -1f;

		public List<HediffDef> makeImmuneTo;

		public List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();

		public List<HediffGiver> hediffGivers = null;

		public List<MentalStateGiver> mentalStateGivers = null;

		public List<StatModifier> statOffsets;

		public float partEfficiencyOffset = 0f;

		public bool partIgnoreMissingHP = false;

		public bool destroyPart;

		public bool AffectsMemory
		{
			get
			{
				return this.forgetMemoryThoughtMtbDays > 0.0 || this.pctConditionalThoughtsNullified > 0.0;
			}
		}

		public bool AffectsSocialInteractions
		{
			get
			{
				return this.opinionOfOthersFactor != 1.0;
			}
		}

		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return HediffStatsUtility.SpecialDisplayStats(this, null);
		}
	}
}
