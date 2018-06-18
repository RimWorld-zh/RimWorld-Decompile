using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B47 RID: 2887
	public class HediffStage
	{
		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06003F4B RID: 16203 RVA: 0x00215044 File Offset: 0x00213444
		public bool AffectsMemory
		{
			get
			{
				return this.forgetMemoryThoughtMtbDays > 0f || this.pctConditionalThoughtsNullified > 0f;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06003F4C RID: 16204 RVA: 0x0021507C File Offset: 0x0021347C
		public bool AffectsSocialInteractions
		{
			get
			{
				return this.opinionOfOthersFactor != 1f;
			}
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x002150A4 File Offset: 0x002134A4
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return HediffStatsUtility.SpecialDisplayStats(this, null);
		}

		// Token: 0x0400299E RID: 10654
		public float minSeverity = 0f;

		// Token: 0x0400299F RID: 10655
		public string label = null;

		// Token: 0x040029A0 RID: 10656
		public bool becomeVisible = true;

		// Token: 0x040029A1 RID: 10657
		public bool lifeThreatening = false;

		// Token: 0x040029A2 RID: 10658
		public TaleDef tale = null;

		// Token: 0x040029A3 RID: 10659
		public float vomitMtbDays = -1f;

		// Token: 0x040029A4 RID: 10660
		public float deathMtbDays = -1f;

		// Token: 0x040029A5 RID: 10661
		public float painFactor = 1f;

		// Token: 0x040029A6 RID: 10662
		public float painOffset = 0f;

		// Token: 0x040029A7 RID: 10663
		public float forgetMemoryThoughtMtbDays = -1f;

		// Token: 0x040029A8 RID: 10664
		public float pctConditionalThoughtsNullified = 0f;

		// Token: 0x040029A9 RID: 10665
		public float opinionOfOthersFactor = 1f;

		// Token: 0x040029AA RID: 10666
		public float hungerRateFactor = 1f;

		// Token: 0x040029AB RID: 10667
		public float hungerRateFactorOffset = 0f;

		// Token: 0x040029AC RID: 10668
		public float restFallFactor = 1f;

		// Token: 0x040029AD RID: 10669
		public float restFallFactorOffset = 0f;

		// Token: 0x040029AE RID: 10670
		public float socialFightChanceFactor = 1f;

		// Token: 0x040029AF RID: 10671
		public float mentalBreakMtbDays = -1f;

		// Token: 0x040029B0 RID: 10672
		public List<HediffDef> makeImmuneTo;

		// Token: 0x040029B1 RID: 10673
		public List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();

		// Token: 0x040029B2 RID: 10674
		public List<HediffGiver> hediffGivers = null;

		// Token: 0x040029B3 RID: 10675
		public List<MentalStateGiver> mentalStateGivers = null;

		// Token: 0x040029B4 RID: 10676
		public List<StatModifier> statOffsets;

		// Token: 0x040029B5 RID: 10677
		public float partEfficiencyOffset = 0f;

		// Token: 0x040029B6 RID: 10678
		public bool partIgnoreMissingHP = false;

		// Token: 0x040029B7 RID: 10679
		public bool destroyPart;
	}
}
