using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse.Grammar;

namespace Verse
{
	public class RulePackDef : Def
	{
		public List<RulePackDef> include;

		private RulePack rulePack;

		[Unsaved]
		private List<Rule> cachedRules;

		[Unsaved]
		private List<Rule> cachedUntranslatedRules;

		public RulePackDef()
		{
		}

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

		public List<Rule> UntranslatedRulesPlusIncludes
		{
			get
			{
				if (this.cachedUntranslatedRules == null)
				{
					this.cachedUntranslatedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedUntranslatedRules.AddRange(this.rulePack.UntranslatedRules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedUntranslatedRules.AddRange(this.include[i].UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.cachedUntranslatedRules;
			}
		}

		public List<Rule> RulesImmediate
		{
			get
			{
				return (this.rulePack == null) ? null : this.rulePack.Rules;
			}
		}

		public List<Rule> UntranslatedRulesImmediate
		{
			get
			{
				return (this.rulePack == null) ? null : this.rulePack.UntranslatedRules;
			}
		}

		public string FirstRuleKeyword
		{
			get
			{
				List<Rule> rulesPlusIncludes = this.RulesPlusIncludes;
				return (!rulesPlusIncludes.Any<Rule>()) ? "none" : rulesPlusIncludes[0].keyword;
			}
		}

		public string FirstUntranslatedRuleKeyword
		{
			get
			{
				List<Rule> untranslatedRulesPlusIncludes = this.UntranslatedRulesPlusIncludes;
				return (!untranslatedRulesPlusIncludes.Any<Rule>()) ? "none" : untranslatedRulesPlusIncludes[0].keyword;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in base.ConfigErrors())
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

		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <err>__1;

			internal int <i>__2;

			internal RulePackDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					IL_15A:
					i++;
					goto IL_168;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						err = enumerator.Current;
						this.$current = err;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (this.include == null)
				{
					goto IL_183;
				}
				i = 0;
				IL_168:
				if (i < this.include.Count)
				{
					if (this.include[i].include != null && this.include[i].include.Contains(this))
					{
						this.$current = "includes other RulePackDef which includes it: " + this.include[i].defName;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_15A;
				}
				IL_183:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				RulePackDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new RulePackDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
