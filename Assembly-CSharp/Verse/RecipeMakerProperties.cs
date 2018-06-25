using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B26 RID: 2854
	public class RecipeMakerProperties
	{
		// Token: 0x040028BD RID: 10429
		public int productCount = 1;

		// Token: 0x040028BE RID: 10430
		public int targetCountAdjustment = 1;

		// Token: 0x040028BF RID: 10431
		public int workAmount = -1;

		// Token: 0x040028C0 RID: 10432
		public StatDef workSpeedStat = null;

		// Token: 0x040028C1 RID: 10433
		public StatDef efficiencyStat = null;

		// Token: 0x040028C2 RID: 10434
		public ThingDef unfinishedThingDef = null;

		// Token: 0x040028C3 RID: 10435
		public ThingFilter defaultIngredientFilter = null;

		// Token: 0x040028C4 RID: 10436
		public List<SkillRequirement> skillRequirements = null;

		// Token: 0x040028C5 RID: 10437
		public SkillDef workSkill = null;

		// Token: 0x040028C6 RID: 10438
		public float workSkillLearnPerTick = 1f;

		// Token: 0x040028C7 RID: 10439
		public EffecterDef effectWorking = null;

		// Token: 0x040028C8 RID: 10440
		public SoundDef soundWorking = null;

		// Token: 0x040028C9 RID: 10441
		public List<ThingDef> recipeUsers = null;

		// Token: 0x040028CA RID: 10442
		public ResearchProjectDef researchPrerequisite = null;

		// Token: 0x040028CB RID: 10443
		[NoTranslate]
		public List<string> factionPrerequisiteTags = null;
	}
}
