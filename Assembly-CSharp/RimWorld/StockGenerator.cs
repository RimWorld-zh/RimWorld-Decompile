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
			if (!this.HandlesThingDef(thingDef))
			{
				priceType = PriceType.Undefined;
				return false;
			}
			priceType = this.price;
			return true;
		}

		protected int RandomCountOf(ThingDef def)
		{
			if (this.countRange.max > 0 && this.totalPriceRange.max <= 0.0)
			{
				return this.countRange.RandomInRange;
			}
			if (this.countRange.max <= 0 && this.totalPriceRange.max > 0.0)
			{
				return Mathf.RoundToInt(this.totalPriceRange.RandomInRange / def.BaseMarketValue);
			}
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
			return randomInRange;
		}
	}
}
