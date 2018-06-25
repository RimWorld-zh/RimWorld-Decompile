using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020003FD RID: 1021
	public class GenStep_RocksFromGrid : GenStep
	{
		// Token: 0x04000AA9 RID: 2729
		private float maxMineableValue = float.MaxValue;

		// Token: 0x04000AAA RID: 2730
		private const int MinRoofedCellsPerGroup = 20;

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x00097F24 File Offset: 0x00096324
		public override int SeedPart
		{
			get
			{
				return 1182952823;
			}
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00097F40 File Offset: 0x00096340
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
				Log.ErrorOnce("Did not get rock def to generate at " + c, 50812, false);
				thingDef = ThingDefOf.Sandstone;
			}
			return thingDef;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00097FDC File Offset: 0x000963DC
		public override void Generate(Map map)
		{
			if (!map.TileInfo.WaterCovered)
			{
				map.regionAndRoomUpdater.Enabled = false;
				float num = 0.7f;
				List<GenStep_RocksFromGrid.RoofThreshold> list = new List<GenStep_RocksFromGrid.RoofThreshold>();
				list.Add(new GenStep_RocksFromGrid.RoofThreshold
				{
					roofDef = RoofDefOf.RoofRockThick,
					minGridVal = num * 1.14f
				});
				list.Add(new GenStep_RocksFromGrid.RoofThreshold
				{
					roofDef = RoofDefOf.RoofRockThin,
					minGridVal = num * 1.04f
				});
				MapGenFloatGrid elevation = MapGenerator.Elevation;
				MapGenFloatGrid caves = MapGenerator.Caves;
				foreach (IntVec3 intVec in map.AllCells)
				{
					float num2 = elevation[intVec];
					if (num2 > num)
					{
						if (caves[intVec] <= 0f)
						{
							ThingDef def = GenStep_RocksFromGrid.RockDefAt(intVec);
							GenSpawn.Spawn(def, intVec, map, WipeMode.Vanish);
						}
						for (int i = 0; i < list.Count; i++)
						{
							if (num2 > list[i].minGridVal)
							{
								map.roofGrid.SetRoof(intVec, list[i].roofDef);
								break;
							}
						}
					}
				}
				BoolGrid visited = new BoolGrid(map);
				List<IntVec3> toRemove = new List<IntVec3>();
				foreach (IntVec3 intVec2 in map.AllCells)
				{
					if (!visited[intVec2])
					{
						if (this.IsNaturalRoofAt(intVec2, map))
						{
							toRemove.Clear();
							map.floodFiller.FloodFill(intVec2, (IntVec3 x) => this.IsNaturalRoofAt(x, map), delegate(IntVec3 x)
							{
								visited[x] = true;
								toRemove.Add(x);
							}, int.MaxValue, false, null);
							if (toRemove.Count < 20)
							{
								for (int j = 0; j < toRemove.Count; j++)
								{
									map.roofGrid.SetRoof(toRemove[j], null);
								}
							}
						}
					}
				}
				GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new GenStep_ScatterLumpsMineable();
				genStep_ScatterLumpsMineable.maxValue = this.maxMineableValue;
				float num3 = 10f;
				switch (Find.WorldGrid[map.Tile].hilliness)
				{
				case Hilliness.Flat:
					num3 = 4f;
					break;
				case Hilliness.SmallHills:
					num3 = 8f;
					break;
				case Hilliness.LargeHills:
					num3 = 11f;
					break;
				case Hilliness.Mountainous:
					num3 = 15f;
					break;
				case Hilliness.Impassable:
					num3 = 16f;
					break;
				}
				genStep_ScatterLumpsMineable.countPer10kCellsRange = new FloatRange(num3, num3);
				genStep_ScatterLumpsMineable.Generate(map);
				map.regionAndRoomUpdater.Enabled = true;
			}
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00098374 File Offset: 0x00096774
		private bool IsNaturalRoofAt(IntVec3 c, Map map)
		{
			return c.Roofed(map) && c.GetRoof(map).isNatural;
		}

		// Token: 0x020003FE RID: 1022
		private class RoofThreshold
		{
			// Token: 0x04000AAB RID: 2731
			public RoofDef roofDef;

			// Token: 0x04000AAC RID: 2732
			public float minGridVal;
		}
	}
}
