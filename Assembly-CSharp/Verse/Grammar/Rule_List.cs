using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BED RID: 3053
	public class Rule_List : Rule
	{
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x0600428E RID: 17038 RVA: 0x002308DC File Offset: 0x0022ECDC
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.strings.Count;
			}
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x00230900 File Offset: 0x0022ED00
		public override string Generate()
		{
			return this.strings.RandomElement<string>();
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x00230920 File Offset: 0x0022ED20
		public override string ToString()
		{
			return this.keyword + "->(list: " + this.strings[0] + " etc)";
		}

		// Token: 0x04002D82 RID: 11650
		private List<string> strings = new List<string>();
	}
}
