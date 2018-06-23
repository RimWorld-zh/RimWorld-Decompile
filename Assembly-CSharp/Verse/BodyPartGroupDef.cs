using System;

namespace Verse
{
	// Token: 0x02000AFE RID: 2814
	public class BodyPartGroupDef : Def
	{
		// Token: 0x04002782 RID: 10114
		[MustTranslate]
		public string labelShort;

		// Token: 0x04002783 RID: 10115
		public int listOrder;

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003E69 RID: 15977 RVA: 0x0020E814 File Offset: 0x0020CC14
		public string LabelShort
		{
			get
			{
				return (!this.labelShort.NullOrEmpty()) ? this.labelShort : this.label;
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003E6A RID: 15978 RVA: 0x0020E84C File Offset: 0x0020CC4C
		public string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}
	}
}
