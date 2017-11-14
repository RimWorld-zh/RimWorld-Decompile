using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_TraderStock : ItemCollectionGenerator
	{
		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			TraderKindDef traderKindDef = parms.traderDef ?? DefDatabase<TraderKindDef>.AllDefsListForReading.RandomElement();
			Faction traderFaction = parms.traderFaction;
			int? tile = parms.tile;
			int forTile = (!tile.HasValue) ? ((Find.AnyPlayerHomeMap == null) ? ((Find.VisibleMap == null) ? (-1) : Find.VisibleMap.Tile) : Find.AnyPlayerHomeMap.Tile) : parms.tile.Value;
			for (int i = 0; i < traderKindDef.stockGenerators.Count; i++)
			{
				StockGenerator stockGenerator = traderKindDef.stockGenerators[i];
				foreach (Thing item in stockGenerator.GenerateThings(forTile))
				{
					if (item.def.tradeability != Tradeability.Stockable)
					{
						Log.Error(traderKindDef + " generated carrying " + item + " which has is not Stockable. Ignoring...");
					}
					else
					{
						item.PostGeneratedForTrader(traderKindDef, forTile, traderFaction);
						outThings.Add(item);
					}
				}
			}
		}

		public float AverageTotalStockValue(TraderKindDef td)
		{
			ItemCollectionGeneratorParams parms = default(ItemCollectionGeneratorParams);
			parms.traderDef = td;
			parms.tile = -1;
			float num = 0f;
			for (int i = 0; i < 50; i++)
			{
				foreach (Thing item in base.Generate(parms))
				{
					num += item.MarketValue * (float)item.stackCount;
				}
			}
			return (float)(num / 50.0);
		}

		public string GenerationDataFor(TraderKindDef td)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(td.defName);
			stringBuilder.AppendLine("Average total market value:" + this.AverageTotalStockValue(td).ToString("F0"));
			ItemCollectionGeneratorParams parms = default(ItemCollectionGeneratorParams);
			parms.traderDef = td;
			parms.tile = -1;
			stringBuilder.AppendLine("Example generated stock:\n\n");
			foreach (Thing item in base.Generate(parms))
			{
				MinifiedThing minifiedThing = item as MinifiedThing;
				Thing thing = (minifiedThing == null) ? item : minifiedThing.InnerThing;
				string labelCap = thing.LabelCap;
				labelCap = labelCap + " [" + (thing.MarketValue * (float)thing.stackCount).ToString("F0") + "]";
				stringBuilder.AppendLine(labelCap);
			}
			return stringBuilder.ToString();
		}
	}
}
