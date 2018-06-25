using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class TaleData_Def : TaleData
	{
		public Def def;

		private string tmpDefName;

		private Type tmpDefType;

		public TaleData_Def()
		{
		}

		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpDefName = ((this.def == null) ? null : this.def.defName);
				this.tmpDefType = ((this.def == null) ? null : this.def.GetType());
			}
			Scribe_Values.Look<string>(ref this.tmpDefName, "defName", null, false);
			Scribe_Values.Look<Type>(ref this.tmpDefType, "defType", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.tmpDefName != null)
			{
				this.def = GenDefDatabase.GetDef(this.tmpDefType, this.tmpDefName, true);
			}
		}

		public override IEnumerable<Rule> GetRules(string prefix)
		{
			if (this.def != null)
			{
				yield return new Rule_String(prefix + "_label", this.def.label);
				yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.def.label));
				yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.def.label));
			}
			yield break;
		}

		public static TaleData_Def GenerateFrom(Def def)
		{
			return new TaleData_Def
			{
				def = def
			};
		}

		[CompilerGenerated]
		private sealed class <GetRules>c__Iterator0 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal string prefix;

			internal TaleData_Def $this;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetRules>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.def != null)
					{
						this.$current = new Rule_String(prefix + "_label", this.def.label);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					this.$current = new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.def.label));
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.def.label));
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			Rule IEnumerator<Rule>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.Rule>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TaleData_Def.<GetRules>c__Iterator0 <GetRules>c__Iterator = new TaleData_Def.<GetRules>c__Iterator0();
				<GetRules>c__Iterator.$this = this;
				<GetRules>c__Iterator.prefix = prefix;
				return <GetRules>c__Iterator;
			}
		}
	}
}
