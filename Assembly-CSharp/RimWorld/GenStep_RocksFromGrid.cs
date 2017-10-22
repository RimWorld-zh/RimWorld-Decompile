using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class GenStep_RocksFromGrid : GenStep
	{
		private class RoofThreshold
		{
			public RoofDef roofDef;

			public float minGridVal;
		}

		private const int MinRoofedCellsPerGroup = 20;

		public static ThingDef RockDefAt(IntVec3 c)
		{
			ThingDef thingDef = null;
			float num = -999999f;
			for (int i = 0; i < RockNoises.rockNoises.Count; i++)
			{
				float value = RockNoises.rockNoises[i].noise.GetValue(c);
				if (value > num)
				{
					thingDef = RockNoises.rockNoises[i].rockDef;
					num = value;
				}
			}
			if (thingDef == null)
			{
				Log.ErrorOnce("Did not get rock def to generate at " + c, 50812);
				thingDef = ThingDefOf.Sandstone;
			}
			return thingDef;
		}

		public override void Generate(Map map)
		{
			if (!map.TileInfo.WaterCovered)
			{
				map.regionAndRoomUpdater.Enabled = false;
				float num = 0.7f;
				List<RoofThreshold> list = new List<RoofThreshold>();
				RoofThreshold roofThreshold = new RoofThreshold();
				roofThreshold.roofDef = RoofDefOf.RoofRockThick;
				roofThreshold.minGridVal = (float)(num * 1.1399999856948853);
				list.Add(roofThreshold);
				RoofThreshold roofThreshold2 = new RoofThreshold();
				roofThreshold2.roofDef = RoofDefOf.RoofRockThin;
				roofThreshold2.minGridVal = (float)(num * 1.0399999618530273);
				list.Add(roofThreshold2);
				MapGenFloatGrid elevation = MapGenerator.Elevation;
				foreach (IntVec3 allCell in map.AllCells)
				{
					float num2 = elevation[allCell];
					if (num2 > num)
					{
						ThingDef def = GenStep_RocksFromGrid.RockDefAt(allCell);
						GenSpawn.Spawn(def, allCell, map);
						int num3 = 0;
						while (num3 < list.Count)
						{
							if (!(num2 > list[num3].minGridVal))
							{
								num3++;
								continue;
							}
							map.roofGrid.SetRoof(allCell, list[num3].roofDef);
							break;
						}
					}
				}
				BoolGrid visited = new BoolGrid(map);
				List<IntVec3> toRemove = new List<IntVec3>();
				foreach (IntVec3 allCell2 in map.AllCells)
				{
					if (!visited[allCell2] && this.IsNaturalRoofAt(allCell2, map))
					{
						toRemove.Clear();
						map.floodFiller.FloodFill(allCell2, (Predicate<IntVec3>)((IntVec3 x) => this.IsNaturalRoofAt(x, map)), (Action<IntVec3>)delegate(IntVec3 x)
						{
							visited[x] = true;
							toRemove.Add(x);
						}, false);
						if (toRemove.Count < 20)
						{
							for (int i = 0; i < toRemove.Count; i++)
							{
								map.roofGrid.SetRoof(toRemove[i], null);
							}
						}
					}
				}
				GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new GenStep_ScatterLumpsMineable();
				float num4 = 10f;
				switch (Find.WorldGrid[map.Tile].hilliness)
				{
				case Hilliness.Flat:
				{
					num4 = 4f;
					break;
				}
				case Hilliness.SmallHills:
				{
					num4 = 8f;
					break;
				}
				case Hilliness.LargeHills:
				{
					num4 = 11f;
					break;
				}
				case Hilliness.Mountainous:
				{
					num4 = 15f;
					break;
				}
				case Hilliness.Impassable:
				{
					num4 = 16f;
					break;
				}
				}
				genStep_ScatterLumpsMineable.countPer10kCellsRange = new FloatRange(num4, num4);
				genStep_ScatterLumpsMineable.Generate(map);
				map.regionAndRoomUpdater.Enabled = true;
			}
		}

		private bool IsNaturalRoofAt(IntVec3 c, Map map)
		{
			return c.Roofed(map) && c.GetRoof(map).isNatural;
		}
	}
}
