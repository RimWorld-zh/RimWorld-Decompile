using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023E RID: 574
	public class CompProperties_Art : CompProperties
	{
		// Token: 0x06000A69 RID: 2665 RVA: 0x0005E6F3 File Offset: 0x0005CAF3
		public CompProperties_Art()
		{
			this.compClass = typeof(CompArt);
		}

		// Token: 0x04000459 RID: 1113
		public RulePackDef nameMaker;

		// Token: 0x0400045A RID: 1114
		public RulePackDef descriptionMaker;

		// Token: 0x0400045B RID: 1115
		public QualityCategory minQualityForArtistic = QualityCategory.Awful;

		// Token: 0x0400045C RID: 1116
		public bool mustBeFullGrave = false;

		// Token: 0x0400045D RID: 1117
		public bool canBeEnjoyedAsArt = false;
	}
}
