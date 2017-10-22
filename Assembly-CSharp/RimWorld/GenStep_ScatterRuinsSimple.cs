using RimWorld.BaseGen;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class GenStep_ScatterRuinsSimple : GenStep_Scatterer
	{
		public IntRange ShedSizeRange = new IntRange(3, 10);

		public IntRange WallLengthRange = new IntRange(4, 14);

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return (byte)(base.CanScatterAt(c, map) ? (c.SupportsStructureType(map, TerrainAffordance.Heavy) ? 1 : 0) : 0) != 0;
		}

		protected bool CanPlaceAncientBuildingInRange(CellRect rect, Map map)
		{
			foreach (IntVec3 cell in rect.Cells)
			{
				if (cell.InBounds(map))
				{
					TerrainDef terrainDef = map.terrainGrid.TerrainAt(cell);
					if (!terrainDef.HasTag("River") && !terrainDef.HasTag("Road"))
					{
						continue;
					}
					return false;
				}
			}
			return true;
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			ThingDef stuffDef = BaseGenUtility.RandomCheapWallStuff(null, true);
			if (Rand.Bool)
			{
				bool @bool = Rand.Bool;
				int randomInRange = this.WallLengthRange.RandomInRange;
				CellRect cellRect = new CellRect(c.x, c.z, (!@bool) ? 1 : randomInRange, @bool ? 1 : randomInRange);
				if (this.CanPlaceAncientBuildingInRange(cellRect.ExpandedBy(1), map))
				{
					this.MakeLongWall(c, map, this.WallLengthRange.RandomInRange, @bool, stuffDef);
				}
			}
			else
			{
				CellRect rect = new CellRect(c.x, c.z, this.ShedSizeRange.RandomInRange, this.ShedSizeRange.RandomInRange).ClipInsideMap(map);
				if (this.CanPlaceAncientBuildingInRange(rect, map))
				{
					RimWorld.BaseGen.BaseGen.globalSettings.map = map;
					RimWorld.BaseGen.BaseGen.symbolStack.Push("ancientRuins", rect);
					RimWorld.BaseGen.BaseGen.Generate();
				}
			}
		}

		private void TrySetCellAsWall(IntVec3 c, Map map, ThingDef stuffDef)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			while (num < thingList.Count)
			{
				if (thingList[num].def.destroyable)
				{
					num++;
					continue;
				}
				return;
			}
			for (int num2 = thingList.Count - 1; num2 >= 0; num2--)
			{
				thingList[num2].Destroy(DestroyMode.Vanish);
			}
			map.terrainGrid.SetTerrain(c, BaseGenUtility.CorrespondingTerrainDef(stuffDef, true));
			Thing newThing = ThingMaker.MakeThing(ThingDefOf.Wall, stuffDef);
			GenSpawn.Spawn(newThing, c, map);
		}

		private void MakeLongWall(IntVec3 start, Map map, int extendDist, bool horizontal, ThingDef stuffDef)
		{
			TerrainDef newTerr = BaseGenUtility.CorrespondingTerrainDef(stuffDef, true);
			IntVec3 intVec = start;
			int num = 0;
			while (num < extendDist && intVec.InBounds(map))
			{
				this.TrySetCellAsWall(intVec, map, stuffDef);
				if (Rand.Chance(0.4f))
				{
					for (int i = 0; i < 9; i++)
					{
						IntVec3 c = intVec + GenAdj.AdjacentCellsAndInside[i];
						if (c.InBounds(map) && Rand.Bool)
						{
							map.terrainGrid.SetTerrain(c, newTerr);
						}
					}
				}
				if (horizontal)
				{
					intVec.x++;
				}
				else
				{
					intVec.z++;
				}
				num++;
			}
		}
	}
}
