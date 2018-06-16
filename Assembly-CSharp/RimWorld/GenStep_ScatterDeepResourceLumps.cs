using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F3 RID: 1011
	public class GenStep_ScatterDeepResourceLumps : GenStep_Scatterer
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x00096F20 File Offset: 0x00095320
		public override int SeedPart
		{
			get
			{
				return 1712041303;
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00096F3C File Offset: 0x0009533C
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

		// Token: 0x0600116C RID: 4460 RVA: 0x00096FB0 File Offset: 0x000953B0
		protected ThingDef ChooseThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00096FEC File Offset: 0x000953EC
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return !base.NearUsedSpot(c, this.minSpacing);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0009701C File Offset: 0x0009541C
		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			ThingDef thingDef = this.ChooseThingDef();
			int numCells = Mathf.CeilToInt((float)thingDef.deepLumpSizeRange.RandomInRange * 1.6f);
			foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(c, map, numCells))
			{
				if (!c2.InNoBuildEdgeArea(map))
				{
					map.deepResourceGrid.SetAt(c2, thingDef, thingDef.deepCountPerCell);
				}
			}
		}

		// Token: 0x04000A9A RID: 2714
		private const float LumpSizeFactor = 1.6f;
	}
}
