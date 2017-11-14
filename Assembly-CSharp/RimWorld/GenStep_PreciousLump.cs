using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GenStep_PreciousLump : GenStep_ScatterLumpsMineable
	{
		public List<ThingOption> mineables;

		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);

		public override void Generate(Map map)
		{
			base.forcedDefToScatter = this.mineables.RandomElementByWeight((ThingOption x) => x.weight).thingDef;
			base.count = 1;
			float randomInRange = this.totalValueRange.RandomInRange;
			float baseMarketValue = base.forcedDefToScatter.building.mineableThing.BaseMarketValue;
			base.forcedLumpSize = Mathf.Max(Mathf.RoundToInt(randomInRange / ((float)base.forcedDefToScatter.building.mineableYield * baseMarketValue)), 1);
			base.Generate(map);
		}

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			base.ScatterAt(c, map, stackCount);
			int minX = base.recentLumpCells.Min((IntVec3 x) => x.x);
			int minZ = base.recentLumpCells.Min((IntVec3 x) => x.z);
			int maxX = base.recentLumpCells.Max((IntVec3 x) => x.x);
			int maxZ = base.recentLumpCells.Max((IntVec3 x) => x.z);
			CellRect var = CellRect.FromLimits(minX, minZ, maxX, maxZ);
			MapGenerator.SetVar("RectOfInterest", var);
		}
	}
}
