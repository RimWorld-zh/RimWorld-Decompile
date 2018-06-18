using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FE RID: 1790
	public class ThingSetMaker_TraderStock : ThingSetMaker
	{
		// Token: 0x060026FC RID: 9980 RVA: 0x0014E644 File Offset: 0x0014CA44
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			TraderKindDef traderKindDef = parms.traderDef ?? DefDatabase<TraderKindDef>.AllDefsListForReading.RandomElement<TraderKindDef>();
			Faction traderFaction = parms.traderFaction;
			int? tile = parms.tile;
			int forTile;
			if (tile != null)
			{
				forTile = parms.tile.Value;
			}
			else if (Find.AnyPlayerHomeMap != null)
			{
				forTile = Find.AnyPlayerHomeMap.Tile;
			}
			else if (Find.CurrentMap != null)
			{
				forTile = Find.CurrentMap.Tile;
			}
			else
			{
				forTile = -1;
			}
			for (int i = 0; i < traderKindDef.stockGenerators.Count; i++)
			{
				StockGenerator stockGenerator = traderKindDef.stockGenerators[i];
				foreach (Thing thing in stockGenerator.GenerateThings(forTile))
				{
					if (!thing.def.tradeability.TraderCanSell())
					{
						Log.Error(string.Concat(new object[]
						{
							traderKindDef,
							" generated carrying ",
							thing,
							" which can't be sold by traders. Ignoring..."
						}), false);
					}
					else
					{
						thing.PostGeneratedForTrader(traderKindDef, forTile, traderFaction);
						outThings.Add(thing);
					}
				}
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x0014E7A8 File Offset: 0x0014CBA8
		public float AverageTotalStockValue(TraderKindDef td)
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = td;
			parms.tile = new int?(-1);
			float num = 0f;
			for (int i = 0; i < 50; i++)
			{
				foreach (Thing thing in base.Generate(parms))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			return num / 50f;
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x0014E85C File Offset: 0x0014CC5C
		public string GenerationDataFor(TraderKindDef td)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(td.defName);
			stringBuilder.AppendLine("Average total market value:" + this.AverageTotalStockValue(td).ToString("F0"));
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = td;
			parms.tile = new int?(-1);
			stringBuilder.AppendLine("Example generated stock:\n\n");
			foreach (Thing thing in base.Generate(parms))
			{
				MinifiedThing minifiedThing = thing as MinifiedThing;
				Thing thing2;
				if (minifiedThing != null)
				{
					thing2 = minifiedThing.InnerThing;
				}
				else
				{
					thing2 = thing;
				}
				string text = thing2.LabelCap;
				text = text + " [" + (thing2.MarketValue * (float)thing2.stackCount).ToString("F0") + "]";
				stringBuilder.AppendLine(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x0014E98C File Offset: 0x0014CD8C
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			if (parms.traderDef == null)
			{
				yield break;
			}
			for (int i = 0; i < parms.traderDef.stockGenerators.Count; i++)
			{
				StockGenerator stock = parms.traderDef.stockGenerators[i];
				foreach (ThingDef t in from x in DefDatabase<ThingDef>.AllDefs
				where x.tradeability.TraderCanSell() && stock.HandlesThingDef(x)
				select x)
				{
					yield return t;
				}
			}
			yield break;
		}
	}
}
