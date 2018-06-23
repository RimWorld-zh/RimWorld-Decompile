using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E8 RID: 744
	public class TraitDegreeData
	{
		// Token: 0x040007DD RID: 2013
		[MustTranslate]
		public string label;

		// Token: 0x040007DE RID: 2014
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabel = null;

		// Token: 0x040007DF RID: 2015
		[MustTranslate]
		public string description;

		// Token: 0x040007E0 RID: 2016
		public int degree = 0;

		// Token: 0x040007E1 RID: 2017
		public float commonality = 1f;

		// Token: 0x040007E2 RID: 2018
		public List<StatModifier> statOffsets;

		// Token: 0x040007E3 RID: 2019
		public List<StatModifier> statFactors;

		// Token: 0x040007E4 RID: 2020
		public ThinkTreeDef thinkTree = null;

		// Token: 0x040007E5 RID: 2021
		public MentalStateDef randomMentalState = null;

		// Token: 0x040007E6 RID: 2022
		public SimpleCurve randomMentalStateMtbDaysMoodCurve = null;

		// Token: 0x040007E7 RID: 2023
		public List<MentalStateDef> disallowedMentalStates = null;

		// Token: 0x040007E8 RID: 2024
		public List<MentalBreakDef> theOnlyAllowedMentalBreaks = null;

		// Token: 0x040007E9 RID: 2025
		public Dictionary<SkillDef, int> skillGains = new Dictionary<SkillDef, int>();

		// Token: 0x040007EA RID: 2026
		public float socialFightChanceFactor = 1f;

		// Token: 0x040007EB RID: 2027
		public float marketValueFactorOffset = 0f;

		// Token: 0x040007EC RID: 2028
		public float randomDiseaseMtbDays = 0f;

		// Token: 0x06000C4B RID: 3147 RVA: 0x0006D337 File Offset: 0x0006B737
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
