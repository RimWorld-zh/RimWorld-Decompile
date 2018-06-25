using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EA RID: 746
	public class TraitDegreeData
	{
		// Token: 0x040007E0 RID: 2016
		[MustTranslate]
		public string label;

		// Token: 0x040007E1 RID: 2017
		[Unsaved]
		[TranslationHandle]
		public string untranslatedLabel = null;

		// Token: 0x040007E2 RID: 2018
		[MustTranslate]
		public string description;

		// Token: 0x040007E3 RID: 2019
		public int degree = 0;

		// Token: 0x040007E4 RID: 2020
		public float commonality = 1f;

		// Token: 0x040007E5 RID: 2021
		public List<StatModifier> statOffsets;

		// Token: 0x040007E6 RID: 2022
		public List<StatModifier> statFactors;

		// Token: 0x040007E7 RID: 2023
		public ThinkTreeDef thinkTree = null;

		// Token: 0x040007E8 RID: 2024
		public MentalStateDef randomMentalState = null;

		// Token: 0x040007E9 RID: 2025
		public SimpleCurve randomMentalStateMtbDaysMoodCurve = null;

		// Token: 0x040007EA RID: 2026
		public List<MentalStateDef> disallowedMentalStates = null;

		// Token: 0x040007EB RID: 2027
		public List<MentalBreakDef> theOnlyAllowedMentalBreaks = null;

		// Token: 0x040007EC RID: 2028
		public Dictionary<SkillDef, int> skillGains = new Dictionary<SkillDef, int>();

		// Token: 0x040007ED RID: 2029
		public float socialFightChanceFactor = 1f;

		// Token: 0x040007EE RID: 2030
		public float marketValueFactorOffset = 0f;

		// Token: 0x040007EF RID: 2031
		public float randomDiseaseMtbDays = 0f;

		// Token: 0x06000C4E RID: 3150 RVA: 0x0006D48F File Offset: 0x0006B88F
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}
	}
}
