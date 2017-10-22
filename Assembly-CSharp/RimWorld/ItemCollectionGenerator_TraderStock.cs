using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_TraderStock : ItemCollectionGenerator
	{
		protected override ItemCollectionGeneratorParams RandomTestParams
		{
			get
			{
				ItemCollectionGeneratorParams randomTestParams = base.RandomTestParams;
				randomTestParams.traderDef = DefDatabase<TraderKindDef>.AllDefsListForReading.RandomElement();
				randomTestParams.forTile = ((Find.VisibleMap == null) ? (-1) : Find.VisibleMap.Tile);
				randomTestParams.forFaction = (Find.FactionManager.RandomAlliedFaction(false, false, true) ?? Find.FactionManager.RandomEnemyFaction(false, false, true));
				return randomTestParams;
			}
		}

		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			TraderKindDef traderDef = parms.traderDef;
			int forTile = parms.forTile;
			Faction forFaction = parms.forFaction;
			for (int i = 0; i < traderDef.stockGenerators.Count; i++)
			{
				StockGenerator stockGenerator = traderDef.stockGenerators[i];
				foreach (Thing item in stockGenerator.GenerateThings(forTile))
				{
					if (item.def.tradeability != Tradeability.Stockable)
					{
						Log.Error(traderDef + " generated carrying " + item + " which has is not Stockable. Ignoring...");
					}
					else
					{
						item.PostGeneratedForTrader(traderDef, forTile, forFaction);
						outThings.Add(item);
					}
				}
			}
		}

		public float AverageTotalStockValue(TraderKindDef td)
		{
			ItemCollectionGeneratorParams parms = new ItemCollectionGeneratorParams
			{
				traderDef = td,
				forTile = -1
			};
			float num = 0f;
			for (int i = 0; i < 50; i++)
			{
				List<Thing>.Enumerator enumerator = base.Generate(parms).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Thing current = enumerator.Current;
						num += current.MarketValue * (float)current.stackCount;
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
			return (float)(num / 50.0);
		}

		public string GenerationDataFor(TraderKindDef td)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(td.defName);
			stringBuilder.AppendLine("Average total market value:" + this.AverageTotalStockValue(td).ToString("F0"));
			ItemCollectionGeneratorParams parms = new ItemCollectionGeneratorParams
			{
				traderDef = td,
				forTile = -1
			};
			stringBuilder.AppendLine("Example generated stock:\n\n");
			List<Thing>.Enumerator enumerator = base.Generate(parms).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Thing current = enumerator.Current;
					MinifiedThing minifiedThing = current as MinifiedThing;
					Thing thing = (minifiedThing == null) ? current : minifiedThing.InnerThing;
					string labelCap = thing.LabelCap;
					labelCap = labelCap + " [" + (thing.MarketValue * (float)thing.stackCount).ToString("F0") + "]";
					stringBuilder.AppendLine(labelCap);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return stringBuilder.ToString();
		}
	}
}
