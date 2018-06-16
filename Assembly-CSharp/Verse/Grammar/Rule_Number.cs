using System;

namespace Verse.Grammar
{
	// Token: 0x02000BEF RID: 3055
	public class Rule_Number : Rule
	{
		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06004296 RID: 17046 RVA: 0x00230A0C File Offset: 0x0022EE0C
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x00230A28 File Offset: 0x0022EE28
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x00230A58 File Offset: 0x0022EE58
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}

		// Token: 0x04002D85 RID: 11653
		private IntRange range = IntRange.zero;

		// Token: 0x04002D86 RID: 11654
		public int selectionWeight = 1;
	}
}
