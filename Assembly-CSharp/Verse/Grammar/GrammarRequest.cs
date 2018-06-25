using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE3 RID: 3043
	public struct GrammarRequest
	{
		// Token: 0x04002D6F RID: 11631
		private List<Rule> rules;

		// Token: 0x04002D70 RID: 11632
		private List<RulePack> includesBare;

		// Token: 0x04002D71 RID: 11633
		private List<RulePackDef> includes;

		// Token: 0x04002D72 RID: 11634
		private Dictionary<string, string> constants;

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004267 RID: 16999 RVA: 0x0022F364 File Offset: 0x0022D764
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

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06004268 RID: 17000 RVA: 0x0022F398 File Offset: 0x0022D798
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

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06004269 RID: 17001 RVA: 0x0022F3CC File Offset: 0x0022D7CC
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

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x0022F400 File Offset: 0x0022D800
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

		// Token: 0x0600426B RID: 17003 RVA: 0x0022F434 File Offset: 0x0022D834
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

		// Token: 0x0600426C RID: 17004 RVA: 0x0022F484 File Offset: 0x0022D884
		public List<Rule> GetRules()
		{
			return this.rules;
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x0022F4A0 File Offset: 0x0022D8A0
		public List<RulePack> GetIncludesBare()
		{
			return this.includesBare;
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x0022F4BC File Offset: 0x0022D8BC
		public List<RulePackDef> GetIncludes()
		{
			return this.includes;
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x0022F4D8 File Offset: 0x0022D8D8
		public Dictionary<string, string> GetConstants()
		{
			return this.constants;
		}
	}
}
