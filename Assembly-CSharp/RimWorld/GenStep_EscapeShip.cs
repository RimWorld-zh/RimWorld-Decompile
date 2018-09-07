using System;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GenStep_EscapeShip : GenStep_Scatterer
	{
		private static readonly IntRange EscapeShipSizeWidth = new IntRange(20, 28);

		private static readonly IntRange EscapeShipSizeHeight = new IntRange(34, 42);

		public GenStep_EscapeShip()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 860042045;
			}
		}

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
			CellRect cellRect = new CellRect(c.x - GenStep_EscapeShip.EscapeShipSizeWidth.min / 2, c.z - GenStep_EscapeShip.EscapeShipSizeHeight.min / 2, GenStep_EscapeShip.EscapeShipSizeWidth.min, GenStep_EscapeShip.EscapeShipSizeHeight.min);
			if (!cellRect.FullyContainedWithin(new CellRect(0, 0, map.Size.x, map.Size.z)))
			{
				return false;
			}
			foreach (IntVec3 c2 in cellRect)
			{
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(c2);
				if (!terrainDef.affordances.Contains(TerrainAffordanceDefOf.Heavy) && (terrainDef.driesTo == null || !terrainDef.driesTo.affordances.Contains(TerrainAffordanceDefOf.Heavy)))
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
			foreach (IntVec3 c2 in rect)
			{
				if (!map.terrainGrid.TerrainAt(c2).affordances.Contains(TerrainAffordanceDefOf.Heavy))
				{
					CompTerrainPumpDry.AffectCell(map, c2);
					for (int i = 0; i < 8; i++)
					{
						Vector3 b = Rand.InsideUnitCircleVec3 * 3f;
						IntVec3 c3 = IntVec3.FromVector3(c2.ToVector3Shifted() + b);
						if (c3.InBounds(map))
						{
							CompTerrainPumpDry.AffectCell(map, c3);
						}
					}
				}
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ship_core", resolveParams);
			BaseGen.Generate();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GenStep_EscapeShip()
		{
		}
	}
}
