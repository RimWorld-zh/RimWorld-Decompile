using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FA RID: 1786
	public class ThingSetMaker_TraderStock : ThingSetMaker
	{
		// Token: 0x060026F4 RID: 9972 RVA: 0x0014E7E8 File Offset: 0x0014CBE8
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

		// Token: 0x060026F5 RID: 9973 RVA: 0x0014E94C File Offset: 0x0014CD4C
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

		// Token: 0x060026F6 RID: 9974 RVA: 0x0014EA00 File Offset: 0x0014CE00
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

		// Token: 0x060026F7 RID: 9975 RVA: 0x0014EB30 File Offset: 0x0014CF30
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
