using System;

namespace Verse.Grammar
{
	// Token: 0x02000BED RID: 3053
	public class Rule_Number : Rule
	{
		// Token: 0x04002D90 RID: 11664
		private IntRange range = IntRange.zero;

		// Token: 0x04002D91 RID: 11665
		public int selectionWeight = 1;

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06004299 RID: 17049 RVA: 0x00231660 File Offset: 0x0022FA60
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x0023167C File Offset: 0x0022FA7C
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x002316AC File Offset: 0x0022FAAC
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}
	}
}
