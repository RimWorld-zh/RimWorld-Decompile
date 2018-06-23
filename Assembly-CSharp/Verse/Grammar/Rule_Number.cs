using System;

namespace Verse.Grammar
{
	// Token: 0x02000BEA RID: 3050
	public class Rule_Number : Rule
	{
		// Token: 0x04002D89 RID: 11657
		private IntRange range = IntRange.zero;

		// Token: 0x04002D8A RID: 11658
		public int selectionWeight = 1;

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06004296 RID: 17046 RVA: 0x002312A4 File Offset: 0x0022F6A4
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x002312C0 File Offset: 0x0022F6C0
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x002312F0 File Offset: 0x0022F6F0
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}
	}
}
