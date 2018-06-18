using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000B72 RID: 2930
	public class RulePackDef : Def
	{
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003FEA RID: 16362 RVA: 0x0021A94C File Offset: 0x00218D4C
		public List<Rule> RulesPlusIncludes
		{
			get
			{
				if (this.cachedRules == null)
				{
					this.cachedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedRules.AddRange(this.rulePack.Rules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedRules.AddRange(this.include[i].RulesPlusIncludes);
						}
					}
				}
				return this.cachedRules;
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003FEB RID: 16363 RVA: 0x0021A9E8 File Offset: 0x00218DE8
		public List<Rule> RulesImmediate
		{
			get
			{
				return (this.rulePack == null) ? null : this.rulePack.Rules;
			}
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x0021AA1C File Offset: 0x00218E1C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.include != null)
			{
				for (int i = 0; i < this.include.Count; i++)
				{
					if (this.include[i].include != null && this.include[i].include.Contains(this))
					{
						yield return "includes other RulePackDef which includes it: " + this.include[i].defName;
					}
				}
			}
			yield break;
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0021AA48 File Offset: 0x00218E48
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}

		// Token: 0x04002ACD RID: 10957
		public List<RulePackDef> include = null;

		// Token: 0x04002ACE RID: 10958
		private RulePack rulePack = null;

		// Token: 0x04002ACF RID: 10959
		[Unsaved]
		private List<Rule> cachedRules = null;
	}
}
