using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000B70 RID: 2928
	public class RulePackDef : Def
	{
		// Token: 0x04002AD2 RID: 10962
		public List<RulePackDef> include = null;

		// Token: 0x04002AD3 RID: 10963
		private RulePack rulePack = null;

		// Token: 0x04002AD4 RID: 10964
		[Unsaved]
		private List<Rule> cachedRules = null;

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003FEF RID: 16367 RVA: 0x0021B0C4 File Offset: 0x002194C4
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
		// (get) Token: 0x06003FF0 RID: 16368 RVA: 0x0021B160 File Offset: 0x00219560
		public List<Rule> RulesImmediate
		{
			get
			{
				return (this.rulePack == null) ? null : this.rulePack.Rules;
			}
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0021B194 File Offset: 0x00219594
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

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0021B1C0 File Offset: 0x002195C0
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}
	}
}
