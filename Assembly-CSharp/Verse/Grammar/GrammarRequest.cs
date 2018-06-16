using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE4 RID: 3044
	public struct GrammarRequest
	{
		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06004260 RID: 16992 RVA: 0x0022E83C File Offset: 0x0022CC3C
		public List<Rule> Rules
		{
			get
			{
				if (this.rules == null)
				{
					this.rules = new List<Rule>();
				}
				return this.rules;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004261 RID: 16993 RVA: 0x0022E870 File Offset: 0x0022CC70
		public List<RulePack> IncludesBare
		{
			get
			{
				if (this.includesBare == null)
				{
					this.includesBare = new List<RulePack>();
				}
				return this.includesBare;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06004262 RID: 16994 RVA: 0x0022E8A4 File Offset: 0x0022CCA4
		public List<RulePackDef> Includes
		{
			get
			{
				if (this.includes == null)
				{
					this.includes = new List<RulePackDef>();
				}
				return this.includes;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06004263 RID: 16995 RVA: 0x0022E8D8 File Offset: 0x0022CCD8
		public Dictionary<string, string> Constants
		{
			get
			{
				if (this.constants == null)
				{
					this.constants = new Dictionary<string, string>();
				}
				return this.constants;
			}
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x0022E90C File Offset: 0x0022CD0C
		public void Clear()
		{
			if (this.rules != null)
			{
				this.rules.Clear();
			}
			if (this.includes != null)
			{
				this.includes.Clear();
			}
			if (this.constants != null)
			{
				this.constants.Clear();
			}
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x0022E95C File Offset: 0x0022CD5C
		public List<Rule> GetRules()
		{
			return this.rules;
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x0022E978 File Offset: 0x0022CD78
		public List<RulePack> GetIncludesBare()
		{
			return this.includesBare;
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x0022E994 File Offset: 0x0022CD94
		public List<RulePackDef> GetIncludes()
		{
			return this.includes;
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x0022E9B0 File Offset: 0x0022CDB0
		public Dictionary<string, string> GetConstants()
		{
			return this.constants;
		}

		// Token: 0x04002D63 RID: 11619
		private List<Rule> rules;

		// Token: 0x04002D64 RID: 11620
		private List<RulePack> includesBare;

		// Token: 0x04002D65 RID: 11621
		private List<RulePackDef> includes;

		// Token: 0x04002D66 RID: 11622
		private Dictionary<string, string> constants;
	}
}
