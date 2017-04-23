using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			foreach (StockGenerator current in this.stockGenerators)
			{
				current.ResolveReferences(this);
			}
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			TraderKindDef.<ConfigErrors>c__Iterator9A <ConfigErrors>c__Iterator9A = new TraderKindDef.<ConfigErrors>c__Iterator9A();
			<ConfigErrors>c__Iterator9A.<>f__this = this;
			TraderKindDef.<ConfigErrors>c__Iterator9A expr_0E = <ConfigErrors>c__Iterator9A;
			expr_0E.$PC = -2;
			return expr_0E;
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
					PriceType result;
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
