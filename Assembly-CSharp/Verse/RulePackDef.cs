using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000B6E RID: 2926
	public class RulePackDef : Def
	{
		// Token: 0x04002AD2 RID: 10962
		public List<RulePackDef> include = null;

		// Token: 0x04002AD3 RID: 10963
		private RulePack rulePack = null;

		// Token: 0x04002AD4 RID: 10964
		[Unsaved]
		private List<Rule> cachedRules = null;

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06003FEC RID: 16364 RVA: 0x0021AFE8 File Offset: 0x002193E8
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

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06003FED RID: 16365 RVA: 0x0021B084 File Offset: 0x00219484
		public List<Rule> RulesImmediate
		{
			get
			{
				return (this.rulePack == null) ? null : this.rulePack.Rules;
			}
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0021B0B8 File Offset: 0x002194B8
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

		// Token: 0x06003FEF RID: 16367 RVA: 0x0021B0E4 File Offset: 0x002194E4
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}
	}
}
