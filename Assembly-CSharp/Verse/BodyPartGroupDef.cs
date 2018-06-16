using System;

namespace Verse
{
	// Token: 0x02000B02 RID: 2818
	public class BodyPartGroupDef : Def
	{
		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003E6B RID: 15979 RVA: 0x0020E404 File Offset: 0x0020C804
		public string LabelShort
		{
			get
			{
				return (!this.labelShort.NullOrEmpty()) ? this.labelShort : this.label;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003E6C RID: 15980 RVA: 0x0020E43C File Offset: 0x0020C83C
		public string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x04002786 RID: 10118
		[MustTranslate]
		public string labelShort;

		// Token: 0x04002787 RID: 10119
		public int listOrder;
	}
}
