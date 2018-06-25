using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		public List<VerbProperties> verbs = null;

		public List<Tool> tools = null;

		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return err;
			}
			if (this.tools != null)
			{
				Tool dupeTool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.label == rhs.label
				select rhs).FirstOrDefault<Tool>();
				if (dupeTool != null)
				{
					yield return string.Format("duplicate hediff tool id {0}", dupeTool.Id);
				}
			}
			yield break;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0(HediffDef parentDef)
		{
			return base.ConfigErrors(parentDef);
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal HediffDef parentDef;

			internal IEnumerator<string> $locvar0;

			internal string <err>__1;

			internal Tool <dupeTool>__2;

			internal HediffCompProperties_VerbGiver $this;

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
					enumerator = base.<ConfigErrors>__BaseCallProxy0(parentDef).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_130;
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
				if (this.tools == null)
				{
					goto IL_131;
				}
				dupeTool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.label == rhs.label
				select rhs).FirstOrDefault<Tool>();
				if (dupeTool == null)
				{
					goto IL_130;
				}
				this.$current = string.Format("duplicate hediff tool id {0}", dupeTool.Id);
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_130:
				IL_131:
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
				HediffCompProperties_VerbGiver.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new HediffCompProperties_VerbGiver.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				<ConfigErrors>c__Iterator.parentDef = parentDef;
				return <ConfigErrors>c__Iterator;
			}

			internal IEnumerable<Tool> <>m__0(Tool lhs)
			{
				return from rhs in this.tools
				where lhs != rhs && lhs.label == rhs.label
				select rhs;
			}

			private sealed class <ConfigErrors>c__AnonStorey1
			{
				internal Tool lhs;

				internal HediffCompProperties_VerbGiver $this;

				public <ConfigErrors>c__AnonStorey1()
				{
				}

				internal bool <>m__0(Tool rhs)
				{
					return this.lhs != rhs && this.lhs.label == rhs.label;
				}
			}
		}
	}
}
