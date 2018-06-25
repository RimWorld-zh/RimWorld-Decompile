using System;

namespace Verse
{
	// Token: 0x02000B01 RID: 2817
	public class BodyPartGroupDef : Def
	{
		// Token: 0x0400278A RID: 10122
		[MustTranslate]
		public string labelShort;

		// Token: 0x0400278B RID: 10123
		public int listOrder;

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003E6D RID: 15981 RVA: 0x0020EC20 File Offset: 0x0020D020
		public string LabelShort
		{
			get
			{
				return (!this.labelShort.NullOrEmpty()) ? this.labelShort : this.label;
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003E6E RID: 15982 RVA: 0x0020EC58 File Offset: 0x0020D058
		public string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}
	}
}
