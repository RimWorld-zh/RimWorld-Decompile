using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE2 RID: 3042
	public struct GrammarRequest
	{
		// Token: 0x04002D68 RID: 11624
		private List<Rule> rules;

		// Token: 0x04002D69 RID: 11625
		private List<RulePack> includesBare;

		// Token: 0x04002D6A RID: 11626
		private List<RulePackDef> includes;

		// Token: 0x04002D6B RID: 11627
		private Dictionary<string, string> constants;

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004267 RID: 16999 RVA: 0x0022F084 File Offset: 0x0022D484
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
		// (get) Token: 0x06004268 RID: 17000 RVA: 0x0022F0B8 File Offset: 0x0022D4B8
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
		// (get) Token: 0x06004269 RID: 17001 RVA: 0x0022F0EC File Offset: 0x0022D4EC
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
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x0022F120 File Offset: 0x0022D520
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

		// Token: 0x0600426B RID: 17003 RVA: 0x0022F154 File Offset: 0x0022D554
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

		// Token: 0x0600426C RID: 17004 RVA: 0x0022F1A4 File Offset: 0x0022D5A4
		public List<Rule> GetRules()
		{
			return this.rules;
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x0022F1C0 File Offset: 0x0022D5C0
		public List<RulePack> GetIncludesBare()
		{
			return this.includesBare;
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x0022F1DC File Offset: 0x0022D5DC
		public List<RulePackDef> GetIncludes()
		{
			return this.includes;
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x0022F1F8 File Offset: 0x0022D5F8
		public Dictionary<string, string> GetConstants()
		{
			return this.constants;
		}
	}
}
