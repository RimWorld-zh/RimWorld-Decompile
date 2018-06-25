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
	public abstract class TaleData : IExposable
	{
		protected TaleData()
		{
		}

		public abstract void ExposeData();

		public virtual IEnumerable<Rule> GetRules(string prefix)
		{
			Log.Error(base.GetType() + " cannot do GetRules with a prefix.", false);
			yield break;
		}

		public virtual IEnumerable<Rule> GetRules()
		{
			Log.Error(base.GetType() + " cannot do GetRules without a prefix.", false);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GetRules>c__Iterator0 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal TaleData $this;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetRules>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
					Log.Error(base.GetType() + " cannot do GetRules with a prefix.", false);
				}
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
				TaleData.<GetRules>c__Iterator0 <GetRules>c__Iterator = new TaleData.<GetRules>c__Iterator0();
				<GetRules>c__Iterator.$this = this;
				return <GetRules>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetRules>c__Iterator1 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal TaleData $this;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetRules>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
					Log.Error(base.GetType() + " cannot do GetRules without a prefix.", false);
				}
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
				TaleData.<GetRules>c__Iterator1 <GetRules>c__Iterator = new TaleData.<GetRules>c__Iterator1();
				<GetRules>c__Iterator.$this = this;
				return <GetRules>c__Iterator;
			}
		}
	}
}
