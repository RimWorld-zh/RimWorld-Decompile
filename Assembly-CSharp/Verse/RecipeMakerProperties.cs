using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B25 RID: 2853
	public class RecipeMakerProperties
	{
		// Token: 0x040028B6 RID: 10422
		public int productCount = 1;

		// Token: 0x040028B7 RID: 10423
		public int targetCountAdjustment = 1;

		// Token: 0x040028B8 RID: 10424
		public int workAmount = -1;

		// Token: 0x040028B9 RID: 10425
		public StatDef workSpeedStat = null;

		// Token: 0x040028BA RID: 10426
		public StatDef efficiencyStat = null;

		// Token: 0x040028BB RID: 10427
		public ThingDef unfinishedThingDef = null;

		// Token: 0x040028BC RID: 10428
		public ThingFilter defaultIngredientFilter = null;

		// Token: 0x040028BD RID: 10429
		public List<SkillRequirement> skillRequirements = null;

		// Token: 0x040028BE RID: 10430
		public SkillDef workSkill = null;

		// Token: 0x040028BF RID: 10431
		public float workSkillLearnPerTick = 1f;

		// Token: 0x040028C0 RID: 10432
		public EffecterDef effectWorking = null;

		// Token: 0x040028C1 RID: 10433
		public SoundDef soundWorking = null;

		// Token: 0x040028C2 RID: 10434
		public List<ThingDef> recipeUsers = null;

		// Token: 0x040028C3 RID: 10435
		public ResearchProjectDef researchPrerequisite = null;

		// Token: 0x040028C4 RID: 10436
		[NoTranslate]
		public List<string> factionPrerequisiteTags = null;
	}
}
