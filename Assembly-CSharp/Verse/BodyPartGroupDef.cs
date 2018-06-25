using System;

namespace Verse
{
	// Token: 0x02000B00 RID: 2816
	public class BodyPartGroupDef : Def
	{
		// Token: 0x04002783 RID: 10115
		[MustTranslate]
		public string labelShort;

		// Token: 0x04002784 RID: 10116
		public int listOrder;

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003E6D RID: 15981 RVA: 0x0020E940 File Offset: 0x0020CD40
		public string LabelShort
		{
			get
			{
				return (!this.labelShort.NullOrEmpty()) ? this.labelShort : this.label;
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003E6E RID: 15982 RVA: 0x0020E978 File Offset: 0x0020CD78
		public string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}
	}
}
