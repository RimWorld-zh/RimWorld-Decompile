using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GenStep_EscapeShip : GenStep_Scatterer
	{
		private static readonly IntRange EscapeShipSizeWidth = new IntRange(20, 28);

		private static readonly IntRange EscapeShipSizeHeight = new IntRange(34, 42);

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
			int x = c.x;
			IntRange escapeShipSizeWidth = GenStep_EscapeShip.EscapeShipSizeWidth;
			int minX = x - escapeShipSizeWidth.min / 2;
			int z = c.z;
			IntRange escapeShipSizeHeight = GenStep_EscapeShip.EscapeShipSizeHeight;
			int minZ = z - escapeShipSizeHeight.min / 2;
			IntRange escapeShipSizeWidth2 = GenStep_EscapeShip.EscapeShipSizeWidth;
			int min = escapeShipSizeWidth2.min;
			IntRange escapeShipSizeHeight2 = GenStep_EscapeShip.EscapeShipSizeHeight;
			CellRect cellRect = new CellRect(minX, minZ, min, escapeShipSizeHeight2.min);
			IntVec3 size = map.Size;
			int x2 = size.x;
			IntVec3 size2 = map.Size;
			if (!cellRect.FullyContainedWithin(new CellRect(0, 0, x2, size2.z)))
			{
				return false;
			}
			foreach (IntVec3 item in cellRect)
			{
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(item);
				if (!terrainDef.affordances.Contains(TerrainAffordance.Heavy) && (terrainDef.driesTo == null || !terrainDef.driesTo.affordances.Contains(TerrainAffordance.Heavy)))
				{
					return false;
				}
			}
			return true;
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			int randomInRange = GenStep_EscapeShip.EscapeShipSizeWidth.RandomInRange;
			int randomInRange2 = GenStep_EscapeShip.EscapeShipSizeHeight.RandomInRange;
			CellRect rect = new CellRect(c.x - randomInRange / 2, c.z - randomInRange2 / 2, randomInRange, randomInRange2);
			rect.ClipInsideMap(map);
			foreach (IntVec3 item in rect)
			{
				if (!map.terrainGrid.TerrainAt(item).affordances.Contains(TerrainAffordance.Heavy))
				{
					CompTerrainPumpDry.AffectCell(map, item);
					for (int i = 0; i < 8; i++)
					{
						Vector2 vector = Random.insideUnitCircle * 3f;
						IntVec3 c2 = IntVec3.FromVector3(item.ToVector3Shifted() + new Vector3(vector.x, 0f, vector.y));
						if (c2.InBounds(map))
						{
							CompTerrainPumpDry.AffectCell(map, c2);
						}
					}
				}
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			RimWorld.BaseGen.BaseGen.globalSettings.map = map;
			RimWorld.BaseGen.BaseGen.symbolStack.Push("ship_core", resolveParams);
			RimWorld.BaseGen.BaseGen.Generate();
		}
	}
}
