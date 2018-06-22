using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F3 RID: 1011
	public class GenStep_ScatterDeepResourceLumps : GenStep_Scatterer
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x0009710C File Offset: 0x0009550C
		public override int SeedPart
		{
			get
			{
				return 1712041303;
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00097128 File Offset: 0x00095528
		public override void Generate(Map map)
		{
			if (!map.TileInfo.WaterCovered)
			{
				int num = base.CalculateFinalCount(map);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec;
					if (!this.TryFindScatterCell(map, out intVec))
					{
						return;
					}
					this.ScatterAt(intVec, map, 1);
					this.usedSpots.Add(intVec);
				}
				this.usedSpots.Clear();
			}
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0009719C File Offset: 0x0009559C
		protected ThingDef ChooseThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x000971D8 File Offset: 0x000955D8
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return !base.NearUsedSpot(c, this.minSpacing);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00097208 File Offset: 0x00095608
		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			ThingDef thingDef = this.ChooseThingDef();
			int numCells = Mathf.CeilToInt((float)thingDef.deepLumpSizeRange.RandomInRange);
			foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(c, map, numCells))
			{
				if (!c2.InNoBuildEdgeArea(map))
				{
					map.deepResourceGrid.SetAt(c2, thingDef, thingDef.deepCountPerCell);
				}
			}
		}
	}
}
