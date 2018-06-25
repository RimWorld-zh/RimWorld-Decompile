using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A7 RID: 679
	public class InspirationDef : Def
	{
		// Token: 0x04000638 RID: 1592
		public Type inspirationClass = typeof(Inspiration);

		// Token: 0x04000639 RID: 1593
		public Type workerClass = typeof(InspirationWorker);

		// Token: 0x0400063A RID: 1594
		public float baseCommonality = 1f;

		// Token: 0x0400063B RID: 1595
		public float baseDurationDays = 1f;

		// Token: 0x0400063C RID: 1596
		public bool allowedOnAnimals;

		// Token: 0x0400063D RID: 1597
		public bool allowedOnNonColonists;

		// Token: 0x0400063E RID: 1598
		public List<StatDef> requiredNonDisabledStats;

		// Token: 0x0400063F RID: 1599
		public List<SkillRequirement> requiredSkills;

		// Token: 0x04000640 RID: 1600
		public List<SkillRequirement> requiredAnySkill;

		// Token: 0x04000641 RID: 1601
		public List<WorkTypeDef> requiredNonDisabledWorkTypes;

		// Token: 0x04000642 RID: 1602
		public List<WorkTypeDef> requiredAnyNonDisabledWorkType;

		// Token: 0x04000643 RID: 1603
		public List<PawnCapacityDef> requiredCapacities;

		// Token: 0x04000644 RID: 1604
		public List<StatModifier> statOffsets;

		// Token: 0x04000645 RID: 1605
		public List<StatModifier> statFactors;

		// Token: 0x04000646 RID: 1606
		[MustTranslate]
		public string beginLetter;

		// Token: 0x04000647 RID: 1607
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x04000648 RID: 1608
		public LetterDef beginLetterDef;

		// Token: 0x04000649 RID: 1609
		[MustTranslate]
		public string endMessage;

		// Token: 0x0400064A RID: 1610
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x0400064B RID: 1611
		[Unsaved]
		private InspirationWorker workerInt = null;

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00066AC4 File Offset: 0x00064EC4
		public InspirationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InspirationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}
	}
}
