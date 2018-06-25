using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E8 RID: 744
	public class TraderKindDef : Def
	{
		// Token: 0x040007CD RID: 1997
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		// Token: 0x040007CE RID: 1998
		public float commonality = 1f;

		// Token: 0x040007CF RID: 1999
		public bool orbital;

		// Token: 0x040007D0 RID: 2000
		public bool requestable = true;

		// Token: 0x040007D1 RID: 2001
		public SimpleCurve commonalityMultFromPopulationIntent;

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x0006CBA4 File Offset: 0x0006AFA4
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

		// Token: 0x06000C44 RID: 3140 RVA: 0x0006CBEC File Offset: 0x0006AFEC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0006CC54 File Offset: 0x0006B054
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

		// Token: 0x06000C46 RID: 3142 RVA: 0x0006CC80 File Offset: 0x0006B080
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

		// Token: 0x06000C47 RID: 3143 RVA: 0x0006CCD4 File Offset: 0x0006B0D4
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
