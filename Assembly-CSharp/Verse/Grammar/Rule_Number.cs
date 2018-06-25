using System;

namespace Verse.Grammar
{
	// Token: 0x02000BEC RID: 3052
	public class Rule_Number : Rule
	{
		// Token: 0x04002D89 RID: 11657
		private IntRange range = IntRange.zero;

		// Token: 0x04002D8A RID: 11658
		public int selectionWeight = 1;

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06004299 RID: 17049 RVA: 0x00231380 File Offset: 0x0022F780
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x0023139C File Offset: 0x0022F79C
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x002313CC File Offset: 0x0022F7CC
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}
	}
}
