using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000B71 RID: 2929
	public class RulePackDef : Def
	{
		// Token: 0x04002AD9 RID: 10969
		public List<RulePackDef> include = null;

		// Token: 0x04002ADA RID: 10970
		private RulePack rulePack = null;

		// Token: 0x04002ADB RID: 10971
		[Unsaved]
		private List<Rule> cachedRules = null;

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003FEF RID: 16367 RVA: 0x0021B3A4 File Offset: 0x002197A4
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

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06003FF0 RID: 16368 RVA: 0x0021B440 File Offset: 0x00219840
		public List<Rule> RulesImmediate
		{
			get
			{
				return (this.rulePack == null) ? null : this.rulePack.Rules;
			}
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0021B474 File Offset: 0x00219874
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

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0021B4A0 File Offset: 0x002198A0
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}
	}
}
