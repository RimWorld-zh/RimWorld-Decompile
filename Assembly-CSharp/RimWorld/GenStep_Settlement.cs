using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	public class GenStep_Settlement : GenStep_Scatterer
	{
		private static readonly IntRange SettlementSizeRange = new IntRange(34, 38);

		public GenStep_Settlement()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 1806208471;
			}
		}

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			bool result;
			if (!base.CanScatterAt(c, map))
			{
				result = false;
			}
			else if (!c.Standable(map))
			{
				result = false;
			}
			else if (c.Roofed(map))
			{
				result = false;
			}
			else if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				result = false;
			}
			else
			{
				int min = GenStep_Settlement.SettlementSizeRange.min;
				CellRect cellRect = new CellRect(c.x - min / 2, c.z - min / 2, min, min);
				result = cellRect.FullyContainedWithin(new CellRect(0, 0, map.Size.x, map.Size.z));
			}
			return result;
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			int randomInRange = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			int randomInRange2 = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			CellRect rect = new CellRect(c.x - randomInRange / 2, c.z - randomInRange2 / 2, randomInRange, randomInRange2);
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			rect.ClipInsideMap(map);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.globalSettings.minBuildings = 1;
			BaseGen.globalSettings.minBarracks = 1;
			BaseGen.symbolStack.Push("settlement", resolveParams);
			BaseGen.Generate();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GenStep_Settlement()
		{
		}
	}
}
