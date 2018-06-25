using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F5 RID: 1013
	public class GenStep_ScatterDeepResourceLumps : GenStep_Scatterer
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x0009726C File Offset: 0x0009566C
		public override int SeedPart
		{
			get
			{
				return 1712041303;
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00097288 File Offset: 0x00095688
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

		// Token: 0x0600116F RID: 4463 RVA: 0x000972FC File Offset: 0x000956FC
		protected ThingDef ChooseThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00097338 File Offset: 0x00095738
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return !base.NearUsedSpot(c, this.minSpacing);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00097368 File Offset: 0x00095768
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
