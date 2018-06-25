using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StockGenerator_MultiDef : StockGenerator
	{
		private List<ThingDef> thingDefs = new List<ThingDef>();

		public StockGenerator_MultiDef()
		{
		}

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			ThingDef td = this.thingDefs.RandomElement<ThingDef>();
			foreach (Thing th in StockGeneratorUtility.TryMakeForStock(td, base.RandomCountOf(td)))
			{
				yield return th;
			}
			yield break;
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			for (int i = 0; i < this.thingDefs.Count; i++)
			{
				if (!this.thingDefs[i].tradeability.TraderCanSell())
				{
					yield return this.thingDefs[i] + " tradeability doesn't allow traders to sell this thing";
				}
			}
			yield break;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0(TraderKindDef parentDef)
		{
			return base.ConfigErrors(parentDef);
		}

		[CompilerGenerated]
		private sealed class <GenerateThings>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal ThingDef <td>__0;

			internal IEnumerator<Thing> $locvar0;

			internal Thing <th>__1;

			internal StockGenerator_MultiDef $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GenerateThings>c__Iterator0()
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
					td = this.thingDefs.RandomElement<ThingDef>();
					enumerator = StockGeneratorUtility.TryMakeForStock(td, base.RandomCountOf(td)).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
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
						th = enumerator.Current;
						this.$current = th;
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
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StockGenerator_MultiDef.<GenerateThings>c__Iterator0 <GenerateThings>c__Iterator = new StockGenerator_MultiDef.<GenerateThings>c__Iterator0();
				<GenerateThings>c__Iterator.$this = this;
				return <GenerateThings>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator1 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal TraderKindDef parentDef;

			internal IEnumerator<string> $locvar0;

			internal string <e>__1;

			internal int <i>__2;

			internal StockGenerator_MultiDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator1()
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
					IL_12A:
					i++;
					goto IL_139;
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
						e = enumerator.Current;
						this.$current = e;
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
				i = 0;
				IL_139:
				if (i >= this.thingDefs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (!this.thingDefs[i].tradeability.TraderCanSell())
					{
						this.$current = this.thingDefs[i] + " tradeability doesn't allow traders to sell this thing";
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_12A;
				}
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
				StockGenerator_MultiDef.<ConfigErrors>c__Iterator1 <ConfigErrors>c__Iterator = new StockGenerator_MultiDef.<ConfigErrors>c__Iterator1();
				<ConfigErrors>c__Iterator.$this = this;
				<ConfigErrors>c__Iterator.parentDef = parentDef;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
