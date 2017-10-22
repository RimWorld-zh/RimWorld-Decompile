using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class TraderKindDef : Def
	{
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		public float commonality = 1f;

		public bool orbital;

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			List<StockGenerator>.Enumerator enumerator = this.stockGenerators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StockGenerator current = enumerator.Current;
					current.ResolveReferences(this);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			List<StockGenerator>.Enumerator enumerator2 = this.stockGenerators.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					StockGenerator stock = enumerator2.Current;
					foreach (string item2 in stock.ConfigErrors(this))
					{
						yield return item2;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
		}

		public bool WillTrade(ThingDef td)
		{
			for (int i = 0; i < this.stockGenerators.Count; i++)
			{
				if (this.stockGenerators[i].HandlesThingDef(td))
				{
					return true;
				}
			}
			return false;
		}

		public PriceType PriceTypeFor(ThingDef thingDef, TradeAction action)
		{
			if (thingDef == ThingDefOf.Silver)
			{
				return PriceType.Undefined;
			}
			if (action == TradeAction.PlayerBuys)
			{
				for (int i = 0; i < this.stockGenerators.Count; i++)
				{
					PriceType result = default(PriceType);
					if (this.stockGenerators[i].TryGetPriceType(thingDef, action, out result))
					{
						return result;
					}
				}
			}
			return PriceType.Normal;
		}
	}
}
