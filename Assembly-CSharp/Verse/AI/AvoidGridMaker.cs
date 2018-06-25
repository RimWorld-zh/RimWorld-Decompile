using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x020009E1 RID: 2529
	public static class AvoidGridMaker
	{
		// Token: 0x04002433 RID: 9267
		private static readonly int TrapRadialCells = GenRadial.NumCellsInRadius(2.9f);

		// Token: 0x060038BE RID: 14526 RVA: 0x001E48A8 File Offset: 0x001E2CA8
		public static void RegenerateAllAvoidGridsFor(Faction faction)
		{
			if (faction.def.canUseAvoidGrid)
			{
				Profiler.BeginSample("RegenerateAllAvoidGridsFor " + faction);
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					AvoidGridMaker.RegenerateAvoidGridsFor(faction, maps[i]);
				}
				Profiler.EndSample();
			}
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x001E490C File Offset: 0x001E2D0C
		public static void RegenerateAvoidGridsFor(Faction faction, Map map)
		{
			if (faction.def.canUseAvoidGrid)
			{
				Profiler.BeginSample("RegenerateAvoidGridsFor " + faction);
				ByteGrid byteGrid;
				if (faction.avoidGridsSmart.TryGetValue(map, out byteGrid))
				{
					byteGrid.Clear(0);
				}
				else
				{
					byteGrid = new ByteGrid(map);
					faction.avoidGridsSmart.Add(map, byteGrid);
				}
				ByteGrid byteGrid2;
				if (faction.avoidGridsBasic.TryGetValue(map, out byteGrid2))
				{
					byteGrid2.Clear(0);
				}
				else
				{
					byteGrid2 = new ByteGrid(map);
					faction.avoidGridsBasic.Add(map, byteGrid2);
				}
				AvoidGridMaker.GenerateAvoidGridInternal(byteGrid, faction, map, AvoidGridMode.Smart);
				AvoidGridMaker.GenerateAvoidGridInternal(byteGrid2, faction, map, AvoidGridMode.Basic);
				Profiler.EndSample();
			}
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x001E49C0 File Offset: 0x001E2DC0
		internal static void Notify_CombatDangerousBuildingDespawned(Building building, Map map)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.HostileTo(Faction.OfPlayer) && map.mapPawns.SpawnedPawnsInFaction(faction).Count > 0)
				{
					AvoidGridMaker.RegenerateAvoidGridsFor(faction, map);
				}
			}
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x001E4A48 File Offset: 0x001E2E48
		private static void GenerateAvoidGridInternal(ByteGrid grid, Faction faction, Map map, AvoidGridMode mode)
		{
			List<TrapMemory> list = faction.TacticalMemory.TrapMemories();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].map == map)
				{
					AvoidGridMaker.PrintAvoidGridAroundTrapLoc(list[i], grid);
				}
			}
			if (mode == AvoidGridMode.Smart)
			{
				List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
				for (int j = 0; j < allBuildingsColonist.Count; j++)
				{
					if (allBuildingsColonist[j].def.building.ai_combatDangerous)
					{
						Building_TurretGun building_TurretGun = allBuildingsColonist[j] as Building_TurretGun;
						if (building_TurretGun != null)
						{
							AvoidGridMaker.PrintAvoidGridAroundTurret(building_TurretGun, grid);
						}
					}
				}
			}
			AvoidGridMaker.ExpandAvoidGridIntoEdifices(grid, map);
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x001E4B10 File Offset: 0x001E2F10
		private static void PrintAvoidGridAroundTrapLoc(TrapMemory mem, ByteGrid avoidGrid)
		{
			Room room = mem.Cell.GetRoom(mem.map, RegionType.Set_Passable);
			for (int i = 0; i < AvoidGridMaker.TrapRadialCells; i++)
			{
				IntVec3 intVec = mem.Cell + GenRadial.RadialPattern[i];
				if (intVec.InBounds(mem.map) && intVec.Walkable(mem.map) && intVec.GetRoom(mem.map, RegionType.Set_Passable) == room)
				{
					float num = (float)Mathf.Max(1, intVec.DistanceToSquared(mem.Cell));
					int num2 = Mathf.Max(1, Mathf.RoundToInt(32f * mem.PowerPercent / num));
					AvoidGridMaker.IncrementAvoidGrid(avoidGrid, intVec, num2);
				}
			}
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x001E4BD8 File Offset: 0x001E2FD8
		private static void PrintAvoidGridAroundTurret(Building_TurretGun tur, ByteGrid avoidGrid)
		{
			int num = GenRadial.NumCellsInRadius(tur.GunCompEq.PrimaryVerb.verbProps.range + 4f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = tur.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(tur.Map) && intVec.Walkable(tur.Map) && GenSight.LineOfSight(intVec, tur.Position, tur.Map, true, null, 0, 0))
				{
					AvoidGridMaker.IncrementAvoidGrid(avoidGrid, intVec, 12);
				}
			}
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x001E4C80 File Offset: 0x001E3080
		private static void IncrementAvoidGrid(ByteGrid avoidGrid, IntVec3 c, int num)
		{
			byte b = avoidGrid[c];
			b = (byte)Mathf.Min(255, (int)b + num);
			avoidGrid[c] = b;
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x001E4CB0 File Offset: 0x001E30B0
		private static void ExpandAvoidGridIntoEdifices(ByteGrid avoidGrid, Map map)
		{
			int numGridCells = map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				if (avoidGrid[i] != 0)
				{
					if (map.edificeGrid[i] == null)
					{
						for (int j = 0; j < 8; j++)
						{
							IntVec3 c = map.cellIndices.IndexToCell(i) + GenAdj.AdjacentCells[j];
							if (c.InBounds(map))
							{
								if (c.GetEdifice(map) != null)
								{
									avoidGrid[c] = (byte)Mathf.Min(255, Mathf.Max((int)avoidGrid[c], (int)avoidGrid[i]));
								}
							}
						}
					}
				}
			}
		}
	}
}
