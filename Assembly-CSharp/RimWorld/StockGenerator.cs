using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class StockGenerator
	{
		[Unsaved]
		public TraderKindDef trader;

		public IntRange countRange = IntRange.zero;

		public FloatRange totalPriceRange = FloatRange.Zero;

		public TechLevel maxTechLevelGenerate = TechLevel.Transcendent;

		public TechLevel maxTechLevelBuy = TechLevel.Transcendent;

		public PriceType price = PriceType.Normal;

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
			int result;
			if (this.countRange.max > 0 && this.totalPriceRange.max <= 0.0)
			{
				result = this.countRange.RandomInRange;
			}
			else if (this.countRange.max <= 0 && this.totalPriceRange.max > 0.0)
			{
				result = Mathf.RoundToInt(this.totalPriceRange.RandomInRange / def.BaseMarketValue);
			}
			else
			{
				int num = 0;
				int randomInRange;
				while (true)
				{
					randomInRange = this.countRange.RandomInRange;
					num++;
					if (num <= 100 && !this.totalPriceRange.Includes((float)randomInRange * def.BaseMarketValue))
					{
						continue;
					}
					break;
				}
				result = randomInRange;
			}
			return result;
		}
	}
}
