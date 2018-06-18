using System;

namespace Verse
{
	// Token: 0x02000B02 RID: 2818
	public class BodyPartGroupDef : Def
	{
		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003E6D RID: 15981 RVA: 0x0020E4D8 File Offset: 0x0020C8D8
		public string LabelShort
		{
			get
			{
				return (!this.labelShort.NullOrEmpty()) ? this.labelShort : this.label;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003E6E RID: 15982 RVA: 0x0020E510 File Offset: 0x0020C910
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
