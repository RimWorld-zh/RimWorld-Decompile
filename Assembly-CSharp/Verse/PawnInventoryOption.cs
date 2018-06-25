using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public class PawnInventoryOption
	{
		public ThingDef thingDef;

		public IntRange countRange = IntRange.one;

		public float choiceChance = 1f;

		public float skipChance;

		public List<PawnInventoryOption> subOptionsTakeAll = null;

		public List<PawnInventoryOption> subOptionsChooseOne = null;

		public PawnInventoryOption()
		{
		}

		public IEnumerable<Thing> GenerateThings()
		{
			if (Rand.Value < this.skipChance)
			{
				yield break;
			}
			if (this.thingDef != null && this.countRange.max > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.thingDef, null);
				thing.stackCount = this.countRange.RandomInRange;
				yield return thing;
			}
			if (this.subOptionsTakeAll != null)
			{
				foreach (PawnInventoryOption opt in this.subOptionsTakeAll)
				{
					foreach (Thing subThing in opt.GenerateThings())
					{
						yield return subThing;
					}
				}
			}
			if (this.subOptionsChooseOne != null)
			{
				PawnInventoryOption chosen = this.subOptionsChooseOne.RandomElementByWeight((PawnInventoryOption o) => o.choiceChance);
				foreach (Thing subThing2 in chosen.GenerateThings())
				{
					yield return subThing2;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GenerateThings>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Thing <thing>__1;

			internal List<PawnInventoryOption>.Enumerator $locvar0;

			internal PawnInventoryOption <opt>__2;

			internal IEnumerator<Thing> $locvar1;

			internal Thing <subThing>__3;

			internal PawnInventoryOption <chosen>__4;

			internal IEnumerator<Thing> $locvar2;

			internal Thing <subThing>__5;

			internal PawnInventoryOption $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<PawnInventoryOption, float> <>f__am$cache0;

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
					if (Rand.Value < this.skipChance)
					{
						return false;
					}
					if (this.thingDef != null && this.countRange.max > 0)
					{
						thing = ThingMaker.MakeThing(this.thingDef, null);
						thing.stackCount = this.countRange.RandomInRange;
						this.$current = thing;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					Block_7:
					try
					{
						switch (num)
						{
						case 2u:
							Block_12:
							try
							{
								switch (num)
								{
								}
								if (enumerator2.MoveNext())
								{
									subThing = enumerator2.Current;
									this.$current = subThing;
									if (!this.$disposing)
									{
										this.$PC = 2;
									}
									flag = true;
									return true;
								}
							}
							finally
							{
								if (!flag)
								{
									if (enumerator2 != null)
									{
										enumerator2.Dispose();
									}
								}
							}
							break;
						}
						if (enumerator.MoveNext())
						{
							opt = enumerator.Current;
							enumerator2 = opt.GenerateThings().GetEnumerator();
							num = 4294967293u;
							goto Block_12;
						}
					}
					finally
					{
						if (!flag)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					goto IL_1CB;
				case 3u:
					Block_10:
					try
					{
						switch (num)
						{
						}
						if (enumerator3.MoveNext())
						{
							subThing2 = enumerator3.Current;
							this.$current = subThing2;
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
					goto IL_2A0;
				default:
					return false;
				}
				if (this.subOptionsTakeAll != null)
				{
					enumerator = this.subOptionsTakeAll.GetEnumerator();
					num = 4294967293u;
					goto Block_7;
				}
				IL_1CB:
				if (this.subOptionsChooseOne != null)
				{
					chosen = this.subOptionsChooseOne.RandomElementByWeight((PawnInventoryOption o) => o.choiceChance);
					enumerator3 = chosen.GenerateThings().GetEnumerator();
					num = 4294967293u;
					goto Block_10;
				}
				IL_2A0:
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
				case 2u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				case 3u:
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
				PawnInventoryOption.<GenerateThings>c__Iterator0 <GenerateThings>c__Iterator = new PawnInventoryOption.<GenerateThings>c__Iterator0();
				<GenerateThings>c__Iterator.$this = this;
				return <GenerateThings>c__Iterator;
			}

			private static float <>m__0(PawnInventoryOption o)
			{
				return o.choiceChance;
			}
		}
	}
}
