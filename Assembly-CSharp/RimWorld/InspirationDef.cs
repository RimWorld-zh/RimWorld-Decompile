using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A5 RID: 677
	public class InspirationDef : Def
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x00066910 File Offset: 0x00064D10
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

		// Token: 0x04000637 RID: 1591
		public Type inspirationClass = typeof(Inspiration);

		// Token: 0x04000638 RID: 1592
		public Type workerClass = typeof(InspirationWorker);

		// Token: 0x04000639 RID: 1593
		public float baseCommonality = 1f;

		// Token: 0x0400063A RID: 1594
		public float baseDurationDays = 1f;

		// Token: 0x0400063B RID: 1595
		public bool allowedOnAnimals;

		// Token: 0x0400063C RID: 1596
		public bool allowedOnNonColonists;

		// Token: 0x0400063D RID: 1597
		public List<StatDef> requiredNonDisabledStats;

		// Token: 0x0400063E RID: 1598
		public List<SkillRequirement> requiredSkills;

		// Token: 0x0400063F RID: 1599
		public List<SkillRequirement> requiredAnySkill;

		// Token: 0x04000640 RID: 1600
		public List<WorkTypeDef> requiredNonDisabledWorkTypes;

		// Token: 0x04000641 RID: 1601
		public List<WorkTypeDef> requiredAnyNonDisabledWorkType;

		// Token: 0x04000642 RID: 1602
		public List<PawnCapacityDef> requiredCapacities;

		// Token: 0x04000643 RID: 1603
		public List<StatModifier> statOffsets;

		// Token: 0x04000644 RID: 1604
		public List<StatModifier> statFactors;

		// Token: 0x04000645 RID: 1605
		[MustTranslate]
		public string beginLetter;

		// Token: 0x04000646 RID: 1606
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x04000647 RID: 1607
		public LetterDef beginLetterDef;

		// Token: 0x04000648 RID: 1608
		[MustTranslate]
		public string endMessage;

		// Token: 0x04000649 RID: 1609
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x0400064A RID: 1610
		[Unsaved]
		private InspirationWorker workerInt = null;
	}
}
