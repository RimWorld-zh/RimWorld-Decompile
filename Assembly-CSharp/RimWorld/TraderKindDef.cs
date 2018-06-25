using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E8 RID: 744
	public class TraderKindDef : Def
	{
		// Token: 0x040007D0 RID: 2000
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		// Token: 0x040007D1 RID: 2001
		public float commonality = 1f;

		// Token: 0x040007D2 RID: 2002
		public bool orbital;

		// Token: 0x040007D3 RID: 2003
		public bool requestable = true;

		// Token: 0x040007D4 RID: 2004
		public SimpleCurve commonalityMultFromPopulationIntent;

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0006CBAC File Offset: 0x0006AFAC
		public float CalculatedCommonality
		{
			get
			{
				float num = this.commonality;
				if (this.commonalityMultFromPopulationIntent != null)
				{
					num *= this.commonalityMultFromPopulationIntent.Evaluate(Find.Storyteller.intenderPopulation.PopulationIntent);
				}
				return num;
			}
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0006CBF4 File Offset: 0x0006AFF4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0006CC5C File Offset: 0x0006B05C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			foreach (StockGenerator stock in this.stockGenerators)
			{
				foreach (string err2 in stock.ConfigErrors(this))
				{
					yield return err2;
				}
			}
			yield break;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0006CC88 File Offset: 0x0006B088
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

		// Token: 0x06000C46 RID: 3142 RVA: 0x0006CCDC File Offset: 0x0006B0DC
		public PriceType PriceTypeFor(ThingDef thingDef, TradeAction action)
		{
			PriceType result;
			if (thingDef == ThingDefOf.Silver)
			{
				result = PriceType.Undefined;
			}
			else
			{
				if (action == TradeAction.PlayerBuys)
				{
					for (int i = 0; i < this.stockGenerators.Count; i++)
					{
						PriceType result2;
						if (this.stockGenerators[i].TryGetPriceType(thingDef, action, out result2))
						{
							return result2;
						}
					}
				}
				result = PriceType.Normal;
			}
			return result;
		}
	}
}
