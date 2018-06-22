using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE0 RID: 3040
	public struct GrammarRequest
	{
		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06004264 RID: 16996 RVA: 0x0022EFA8 File Offset: 0x0022D3A8
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

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06004265 RID: 16997 RVA: 0x0022EFDC File Offset: 0x0022D3DC
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

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06004266 RID: 16998 RVA: 0x0022F010 File Offset: 0x0022D410
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

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06004267 RID: 16999 RVA: 0x0022F044 File Offset: 0x0022D444
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

		// Token: 0x06004268 RID: 17000 RVA: 0x0022F078 File Offset: 0x0022D478
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

		// Token: 0x06004269 RID: 17001 RVA: 0x0022F0C8 File Offset: 0x0022D4C8
		public List<Rule> GetRules()
		{
			return this.rules;
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x0022F0E4 File Offset: 0x0022D4E4
		public List<RulePack> GetIncludesBare()
		{
			return this.includesBare;
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x0022F100 File Offset: 0x0022D500
		public List<RulePackDef> GetIncludes()
		{
			return this.includes;
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x0022F11C File Offset: 0x0022D51C
		public Dictionary<string, string> GetConstants()
		{
			return this.constants;
		}

		// Token: 0x04002D68 RID: 11624
		private List<Rule> rules;

		// Token: 0x04002D69 RID: 11625
		private List<RulePack> includesBare;

		// Token: 0x04002D6A RID: 11626
		private List<RulePackDef> includes;

		// Token: 0x04002D6B RID: 11627
		private Dictionary<string, string> constants;
	}
}
