using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.Grammar;

namespace Verse
{
	public class RulePackDef : Def
	{
		public List<RulePackDef> include;

		private RulePack rulePack;

		private List<Rule> cachedRules;

		public List<Rule> Rules
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
							this.cachedRules.AddRange(this.include[i].Rules);
						}
					}
				}
				return this.cachedRules;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			RulePackDef.<ConfigErrors>c__Iterator1D8 <ConfigErrors>c__Iterator1D = new RulePackDef.<ConfigErrors>c__Iterator1D8();
			<ConfigErrors>c__Iterator1D.<>f__this = this;
			RulePackDef.<ConfigErrors>c__Iterator1D8 expr_0E = <ConfigErrors>c__Iterator1D;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}
	}
}
