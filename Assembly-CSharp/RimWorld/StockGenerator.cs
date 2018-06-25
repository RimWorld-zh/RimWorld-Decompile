using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class StockGenerator
	{
		[Unsaved]
		public TraderKindDef trader;

		public IntRange countRange = IntRange.zero;

		public List<ThingDefCountRangeClass> customCountRanges;

		public FloatRange totalPriceRange = FloatRange.Zero;

		public TechLevel maxTechLevelGenerate = TechLevel.Archotech;

		public TechLevel maxTechLevelBuy = TechLevel.Archotech;

		public PriceType price = PriceType.Normal;

		protected StockGenerator()
		{
		}

		public virtual void ResolveReferences(TraderKindDef trader)
		{
			this.trader = trader;
		}

		public virtual IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			yield break;
		}

		public abstract IEnumerable<Thing> GenerateThings(int forTile);

		public abstract bool HandlesThingDef(ThingDef thingDef);

		public bool TryGetPriceType(ThingDef thingDef, TradeAction action, out PriceType priceType)
		{
			bool result;
			if (!this.HandlesThingDef(thingDef))
			{
				priceType = PriceType.Undefined;
				result = false;
			}
			else
			{
				priceType = this.price;
				result = true;
			}
			return result;
		}

		protected int RandomCountOf(ThingDef def)
		{
			IntRange intRange = this.countRange;
			if (this.customCountRanges != null)
			{
				for (int i = 0; i < this.customCountRanges.Count; i++)
				{
					if (this.customCountRanges[i].thingDef == def)
					{
						intRange = this.customCountRanges[i].countRange;
						break;
					}
				}
			}
			int result;
			if (intRange.max <= 0 && this.totalPriceRange.max <= 0f)
			{
				result = 0;
			}
			else if (intRange.max > 0 && this.totalPriceRange.max <= 0f)
			{
				result = intRange.RandomInRange;
			}
			else if (intRange.max <= 0 && this.totalPriceRange.max > 0f)
			{
				result = Mathf.RoundToInt(this.totalPriceRange.RandomInRange / def.BaseMarketValue);
			}
			else
			{
				int num = 0;
				int randomInRange;
				do
				{
					randomInRange = intRange.RandomInRange;
					num++;
					if (num > 100)
					{
						break;
					}
				}
				while (!this.totalPriceRange.Includes((float)randomInRange * def.BaseMarketValue));
				result = randomInRange;
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
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
				return new StockGenerator.<ConfigErrors>c__Iterator0();
			}
		}
	}
}
