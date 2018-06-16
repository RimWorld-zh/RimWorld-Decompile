using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E8 RID: 744
	public class TraitDegreeData
	{
		// Token: 0x040007DC RID: 2012
		[MustTranslate]
		public string label;

		// Token: 0x040007DD RID: 2013
		[MustTranslate]
		public string description;

		// Token: 0x040007DE RID: 2014
		public int degree = 0;

		// Token: 0x040007DF RID: 2015
		public float commonality = 1f;

		// Token: 0x040007E0 RID: 2016
		public List<StatModifier> statOffsets;

		// Token: 0x040007E1 RID: 2017
		public List<StatModifier> statFactors;

		// Token: 0x040007E2 RID: 2018
		public ThinkTreeDef thinkTree = null;

		// Token: 0x040007E3 RID: 2019
		public MentalStateDef randomMentalState = null;

		// Token: 0x040007E4 RID: 2020
		public SimpleCurve randomMentalStateMtbDaysMoodCurve = null;

		// Token: 0x040007E5 RID: 2021
		public List<MentalStateDef> disallowedMentalStates = null;

		// Token: 0x040007E6 RID: 2022
		public List<MentalBreakDef> theOnlyAllowedMentalBreaks = null;

		// Token: 0x040007E7 RID: 2023
		public Dictionary<SkillDef, int> skillGains = new Dictionary<SkillDef, int>();

		// Token: 0x040007E8 RID: 2024
		public float socialFightChanceFactor = 1f;

		// Token: 0x040007E9 RID: 2025
		public float marketValueFactorOffset = 0f;

		// Token: 0x040007EA RID: 2026
		public float randomDiseaseMtbDays = 0f;
	}
}
