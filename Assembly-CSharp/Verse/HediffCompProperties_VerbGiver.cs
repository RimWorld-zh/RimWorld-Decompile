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

		public override void PostLoad()
		{
			base.PostLoad();
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
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
				where lhs != rhs && lhs.id == rhs.id
				select rhs).FirstOrDefault<Tool>();
				if (dupeTool != null)
				{
					yield return string.Format("duplicate hediff tool id {0}", dupeTool.id);
				}
				foreach (Tool t in this.tools)
				{
					foreach (string e in t.ConfigErrors())
					{
						yield return e;
					}
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

			internal List<Tool>.Enumerator $locvar1;

			internal Tool <t>__3;

			internal IEnumerator<string> $locvar2;

			internal string <e>__4;

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
					goto IL_134;
				case 3u:
					goto IL_14E;
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
					goto IL_22E;
				}
				dupeTool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.id == rhs.id
				select rhs).FirstOrDefault<Tool>();
				if (dupeTool != null)
				{
					this.$current = string.Format("duplicate hediff tool id {0}", dupeTool.id);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_134:
				enumerator2 = this.tools.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_14E:
					switch (num)
					{
					case 3u:
						Block_14:
						try
						{
							switch (num)
							{
							}
							if (enumerator3.MoveNext())
							{
								e = enumerator3.Current;
								this.$current = e;
								if (!this.$disposing)
								{
									this.$PC = 3;
								}
								flag = true;
								return true;
							}
						}
						finally
						{
							if (!flag)
							{
								if (enumerator3 != null)
								{
									enumerator3.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator2.MoveNext())
					{
						t = enumerator2.Current;
						enumerator3 = t.ConfigErrors().GetEnumerator();
						num = 4294967293u;
						goto Block_14;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				IL_22E:
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
				case 3u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator3 != null)
							{
								enumerator3.Dispose();
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
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
				where lhs != rhs && lhs.id == rhs.id
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
					return this.lhs != rhs && this.lhs.id == rhs.id;
				}
			}
		}
	}
}
