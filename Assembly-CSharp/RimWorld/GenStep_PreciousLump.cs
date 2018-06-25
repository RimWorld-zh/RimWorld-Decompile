using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200040F RID: 1039
	public class GenStep_PreciousLump : GenStep_ScatterLumpsMineable
	{
		// Token: 0x04000AD9 RID: 2777
		public List<ThingOption> mineables;

		// Token: 0x04000ADA RID: 2778
		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x0009B124 File Offset: 0x00099524
		public override int SeedPart
		{
			get
			{
				return 1634184421;
			}
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0009B140 File Offset: 0x00099540
		public override void Generate(Map map)
		{
			this.forcedDefToScatter = this.mineables.RandomElementByWeight((ThingOption x) => x.weight).thingDef;
			this.count = 1;
			float randomInRange = this.totalValueRange.RandomInRange;
			float baseMarketValue = this.forcedDefToScatter.building.mineableThing.BaseMarketValue;
			this.forcedLumpSize = Mathf.Max(Mathf.RoundToInt(randomInRange / ((float)this.forcedDefToScatter.building.mineableYield * baseMarketValue)), 1);
			base.Generate(map);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0009B1D8 File Offset: 0x000995D8
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0009B204 File Offset: 0x00099604
		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			base.ScatterAt(c, map, stackCount);
			int minX = this.recentLumpCells.Min((IntVec3 x) => x.x);
			int minZ = this.recentLumpCells.Min((IntVec3 x) => x.z);
			int maxX = this.recentLumpCells.Max((IntVec3 x) => x.x);
			int maxZ = this.recentLumpCells.Max((IntVec3 x) => x.z);
			CellRect var = CellRect.FromLimits(minX, minZ, maxX, maxZ);
			MapGenerator.SetVar<CellRect>("RectOfInterest", var);
		}
	}
}
