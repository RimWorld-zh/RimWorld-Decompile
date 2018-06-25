using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000240 RID: 576
	public class CompProperties_Art : CompProperties
	{
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

		// Token: 0x06000A6A RID: 2666 RVA: 0x0005E89B File Offset: 0x0005CC9B
		public CompProperties_Art()
		{
			this.compClass = typeof(CompArt);
		}
	}
}
