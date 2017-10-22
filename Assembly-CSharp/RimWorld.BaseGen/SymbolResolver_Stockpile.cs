using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Stockpile : SymbolResolver
	{
		private const float FreeCellsFraction = 0.41f;

		private List<IntVec3> cells = new List<IntVec3>();

		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			if (rp.stockpileConcreteContents != null && rp.stockpileConcreteContents.Any())
			{
				this.CalculateFreeCells(rp.rect, 0f);
				int num = 0;
				int num2 = rp.stockpileConcreteContents.Count - 1;
				while (num2 >= 0 && num < this.cells.Count)
				{
					GenSpawn.Spawn(rp.stockpileConcreteContents[num2], this.cells[num], map);
					num++;
					num2--;
				}
				for (int num3 = rp.stockpileConcreteContents.Count - 1; num3 >= 0; num3--)
				{
					if (!rp.stockpileConcreteContents[num3].Spawned)
					{
						rp.stockpileConcreteContents[num3].Destroy(DestroyMode.Vanish);
					}
				}
				rp.stockpileConcreteContents.Clear();
			}
			else
			{
				ItemCollectionGeneratorDef itemCollectionGeneratorDef = rp.itemCollectionGeneratorDef ?? Rand.Element(ItemCollectionGeneratorDefOf.RandomGeneralGoods, ItemCollectionGeneratorDefOf.Weapons, ItemCollectionGeneratorDefOf.Apparel, ItemCollectionGeneratorDefOf.RawResources);
				ItemCollectionGeneratorParams? itemCollectionGeneratorParams = rp.itemCollectionGeneratorParams;
				ItemCollectionGeneratorParams value;
				if (itemCollectionGeneratorParams.HasValue)
				{
					value = rp.itemCollectionGeneratorParams.Value;
				}
				else
				{
					this.CalculateFreeCells(rp.rect, 0.41f);
					value = new ItemCollectionGeneratorParams
					{
						count = this.cells.Count,
						techLevel = ((rp.faction == null) ? TechLevel.Spacer : rp.faction.def.techLevel)
					};
					if (itemCollectionGeneratorDef.Worker is ItemCollectionGenerator_Standard)
					{
						float? stockpileMarketValue = rp.stockpileMarketValue;
						float num4 = value.totalMarketValue = ((!stockpileMarketValue.HasValue) ? Mathf.Min((float)((float)this.cells.Count * 120.0), 1800f) : stockpileMarketValue.Value);
					}
				}
				ResolveParams resolveParams = rp;
				resolveParams.itemCollectionGeneratorDef = itemCollectionGeneratorDef;
				resolveParams.itemCollectionGeneratorParams = new ItemCollectionGeneratorParams?(value);
				BaseGen.symbolStack.Push("itemCollection", resolveParams);
			}
		}

		private void CalculateFreeCells(CellRect rect, float freeCellsFraction)
		{
			Map map = BaseGen.globalSettings.map;
			this.cells.Clear();
			foreach (IntVec3 item in rect)
			{
				if (item.Standable(map) && item.GetFirstItem(map) == null)
				{
					this.cells.Add(item);
				}
			}
			int num = (int)(freeCellsFraction * (float)this.cells.Count);
			for (int num2 = 0; num2 < num; num2++)
			{
				this.cells.RemoveAt(Rand.Range(0, this.cells.Count));
			}
			this.cells.Shuffle();
		}
	}
}
