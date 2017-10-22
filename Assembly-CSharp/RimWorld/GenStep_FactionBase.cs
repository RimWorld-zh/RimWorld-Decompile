using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	public class GenStep_FactionBase : GenStep_Scatterer
	{
		private static readonly IntRange FactionBaseSizeRange = new IntRange(34, 38);

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				return false;
			}
			IntRange factionBaseSizeRange = GenStep_FactionBase.FactionBaseSizeRange;
			int min = factionBaseSizeRange.min;
			CellRect cellRect = new CellRect(c.x - min / 2, c.z - min / 2, min, min);
			IntVec3 size = map.Size;
			int x = size.x;
			IntVec3 size2 = map.Size;
			if (!cellRect.FullyContainedWithin(new CellRect(0, 0, x, size2.z)))
			{
				return false;
			}
			return true;
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			int randomInRange = GenStep_FactionBase.FactionBaseSizeRange.RandomInRange;
			int randomInRange2 = GenStep_FactionBase.FactionBaseSizeRange.RandomInRange;
			CellRect rect = new CellRect(c.x - randomInRange / 2, c.z - randomInRange2 / 2, randomInRange, randomInRange2);
			Faction faction = (map.ParentFaction != null && map.ParentFaction != Faction.OfPlayer) ? map.ParentFaction : Find.FactionManager.RandomEnemyFaction(false, false, true);
			rect.ClipInsideMap(map);
			ResolveParams resolveParams = new ResolveParams
			{
				rect = rect,
				faction = faction
			};
			RimWorld.BaseGen.BaseGen.globalSettings.map = map;
			RimWorld.BaseGen.BaseGen.globalSettings.minBuildings = 1;
			RimWorld.BaseGen.BaseGen.globalSettings.minBarracks = 1;
			RimWorld.BaseGen.BaseGen.symbolStack.Push("factionBase", resolveParams);
			RimWorld.BaseGen.BaseGen.Generate();
		}
	}
}
