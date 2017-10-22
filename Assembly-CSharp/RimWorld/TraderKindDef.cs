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
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string err2 = enumerator.Current;
					yield return err2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				using (IEnumerator<string> enumerator3 = stockGenerator.ConfigErrors(this).GetEnumerator())
				{
					if (enumerator3.MoveNext())
					{
						string err = enumerator3.Current;
						yield return err;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_01c0:
			/*Error near IL_01c1: Unexpected return in MoveNext()*/;
		}

		public bool WillTrade(ThingDef td)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.stockGenerators.Count)
				{
					if (this.stockGenerators[num].HandlesThingDef(td))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public PriceType PriceTypeFor(ThingDef thingDef, TradeAction action)
		{
			PriceType result;
			PriceType priceType = default(PriceType);
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
						if (this.stockGenerators[i].TryGetPriceType(thingDef, action, out priceType))
							goto IL_003d;
					}
				}
				result = PriceType.Normal;
			}
			goto IL_0062;
			IL_0062:
			return result;
			IL_003d:
			result = priceType;
			goto IL_0062;
		}
	}
}
