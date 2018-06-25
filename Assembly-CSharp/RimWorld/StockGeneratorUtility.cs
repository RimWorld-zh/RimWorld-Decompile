using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StockGeneratorUtility
	{
		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache0;

		public StockGeneratorUtility()
		{
		}

		public static IEnumerable<Thing> TryMakeForStock(ThingDef thingDef, int count)
		{
			if (thingDef.MadeFromStuff)
			{
				for (int i = 0; i < count; i++)
				{
					Thing th = StockGeneratorUtility.TryMakeForStockSingle(thingDef, 1);
					if (th != null)
					{
						yield return th;
					}
				}
			}
			else
			{
				Thing th2 = StockGeneratorUtility.TryMakeForStockSingle(thingDef, count);
				if (th2 != null)
				{
					yield return th2;
				}
			}
			yield break;
		}

		public static Thing TryMakeForStockSingle(ThingDef thingDef, int stackCount)
		{
			Thing result;
			if (stackCount <= 0)
			{
				result = null;
			}
			else if (!thingDef.tradeability.TraderCanSell())
			{
				Log.Error("Tried to make non-trader-sellable thing for trader stock: " + thingDef, false);
				result = null;
			}
			else
			{
				ThingDef stuff = null;
				if (thingDef.MadeFromStuff)
				{
					if (!(from x in GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined)
					where !PawnWeaponGenerator.IsDerpWeapon(thingDef, x)
					select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
					{
						stuff = GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined);
					}
				}
				Thing thing = ThingMaker.MakeThing(thingDef, stuff);
				thing.stackCount = stackCount;
				result = thing;
			}
			return result;
		}

		[CompilerGenerated]
		private static float <TryMakeForStockSingle>m__0(ThingDef x)
		{
			return x.stuffProps.commonality;
		}

		[CompilerGenerated]
		private sealed class <TryMakeForStock>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal ThingDef thingDef;

			internal int <i>__1;

			internal int count;

			internal Thing <th>__2;

			internal Thing <th>__3;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <TryMakeForStock>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (thingDef.MadeFromStuff)
					{
						i = 0;
					}
					else
					{
						th2 = StockGeneratorUtility.TryMakeForStockSingle(thingDef, count);
						if (th2 != null)
						{
							this.$current = th2;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							return true;
						}
						goto IL_EA;
					}
					break;
				case 1u:
					IL_81:
					i++;
					break;
				case 2u:
					goto IL_EA;
				default:
					return false;
				}
				if (i < count)
				{
					th = StockGeneratorUtility.TryMakeForStockSingle(thingDef, 1);
					if (th != null)
					{
						this.$current = th;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_81;
				}
				IL_EA:
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
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StockGeneratorUtility.<TryMakeForStock>c__Iterator0 <TryMakeForStock>c__Iterator = new StockGeneratorUtility.<TryMakeForStock>c__Iterator0();
				<TryMakeForStock>c__Iterator.thingDef = thingDef;
				<TryMakeForStock>c__Iterator.count = count;
				return <TryMakeForStock>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <TryMakeForStockSingle>c__AnonStorey1
		{
			internal ThingDef thingDef;

			public <TryMakeForStockSingle>c__AnonStorey1()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return !PawnWeaponGenerator.IsDerpWeapon(this.thingDef, x);
			}
		}
	}
}
