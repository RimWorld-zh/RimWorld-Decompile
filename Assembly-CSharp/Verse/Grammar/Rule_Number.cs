using System;

namespace Verse.Grammar
{
	// Token: 0x02000BEE RID: 3054
	public class Rule_Number : Rule
	{
		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06004294 RID: 17044 RVA: 0x002309F0 File Offset: 0x0022EDF0
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x00230A0C File Offset: 0x0022EE0C
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x00230A3C File Offset: 0x0022EE3C
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}

		// Token: 0x04002D83 RID: 11651
		private IntRange range = IntRange.zero;

		// Token: 0x04002D84 RID: 11652
		public int selectionWeight = 1;
	}
}
