using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D4 RID: 980
	public class SymbolResolver_Stockpile : SymbolResolver
	{
		// Token: 0x04000A44 RID: 2628
		private List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x04000A45 RID: 2629
		private const float FreeCellsFraction = 0.45f;

		// Token: 0x060010DC RID: 4316 RVA: 0x0008FAA0 File Offset: 0x0008DEA0
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			if (rp.stockpileConcreteContents != null)
			{
				this.CalculateFreeCells(rp.rect, 0f);
				int num = 0;
				for (int i = rp.stockpileConcreteContents.Count - 1; i >= 0; i--)
				{
					if (num >= this.cells.Count)
					{
						break;
					}
					GenSpawn.Spawn(rp.stockpileConcreteContents[i], this.cells[num], map, WipeMode.Vanish);
					num++;
				}
				for (int j = rp.stockpileConcreteContents.Count - 1; j >= 0; j--)
				{
					if (!rp.stockpileConcreteContents[j].Spawned)
					{
						rp.stockpileConcreteContents[j].Destroy(DestroyMode.Vanish);
					}
				}
				rp.stockpileConcreteContents.Clear();
			}
			else
			{
				this.CalculateFreeCells(rp.rect, 0.45f);
				ThingSetMakerDef thingSetMakerDef = rp.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile;
				ThingSetMakerParams? thingSetMakerParams = rp.thingSetMakerParams;
				ThingSetMakerParams value;
				if (thingSetMakerParams != null)
				{
					value = rp.thingSetMakerParams.Value;
				}
				else
				{
					value = default(ThingSetMakerParams);
					value.techLevel = new TechLevel?((rp.faction == null) ? TechLevel.Undefined : rp.faction.def.techLevel);
					value.validator = ((ThingDef x) => rp.faction == null || x.techLevel >= rp.faction.def.techLevel || !x.IsWeapon || x.GetStatValueAbstract(StatDefOf.MarketValue, GenStuff.DefaultStuffFor(x)) >= 100f);
					float? stockpileMarketValue = rp.stockpileMarketValue;
					float num2 = (stockpileMarketValue == null) ? Mathf.Min((float)this.cells.Count * 130f, 1800f) : stockpileMarketValue.Value;
					value.totalMarketValueRange = new FloatRange?(new FloatRange(num2, num2));
				}
				IntRange? countRange = value.countRange;
				if (countRange == null)
				{
					value.countRange = new IntRange?(new IntRange(this.cells.Count, this.cells.Count));
				}
				ResolveParams rp2 = rp;
				rp2.thingSetMakerDef = thingSetMakerDef;
				rp2.thingSetMakerParams = new ThingSetMakerParams?(value);
				BaseGen.symbolStack.Push("thingSet", rp2);
			}
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0008FD44 File Offset: 0x0008E144
		private void CalculateFreeCells(CellRect rect, float freeCellsFraction)
		{
			Map map = BaseGen.globalSettings.map;
			this.cells.Clear();
			foreach (IntVec3 intVec in rect)
			{
				if (intVec.Standable(map) && intVec.GetFirstItem(map) == null)
				{
					this.cells.Add(intVec);
				}
			}
			int num = (int)(freeCellsFraction * (float)this.cells.Count);
			for (int i = 0; i < num; i++)
			{
				this.cells.RemoveAt(Rand.Range(0, this.cells.Count));
			}
			this.cells.Shuffle<IntVec3>();
		}
	}
}
