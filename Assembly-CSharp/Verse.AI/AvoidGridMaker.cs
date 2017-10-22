#define ENABLE_PROFILER
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI.Group;

namespace Verse.AI
{
	public static class AvoidGridMaker
	{
		private static readonly int TrapRadialCells = GenRadial.NumCellsInRadius(2.9f);

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

		public static void RegenerateAvoidGridsFor(Faction faction, Map map)
		{
			if (faction.def.canUseAvoidGrid)
			{
				Profiler.BeginSample("RegenerateAvoidGridsFor " + faction);
				ByteGrid byteGrid = default(ByteGrid);
				if (faction.avoidGridsSmart.TryGetValue(map, out byteGrid))
				{
					byteGrid.Clear((byte)0);
				}
				else
				{
					byteGrid = new ByteGrid(map);
					faction.avoidGridsSmart.Add(map, byteGrid);
				}
				ByteGrid byteGrid2 = default(ByteGrid);
				if (faction.avoidGridsBasic.TryGetValue(map, out byteGrid2))
				{
					byteGrid2.Clear((byte)0);
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

		internal static void Notify_CombatDangerousBuildingDespawned(Building building, Map map)
		{
			foreach (Faction allFaction in Find.FactionManager.AllFactions)
			{
				if (allFaction.HostileTo(Faction.OfPlayer) && map.mapPawns.SpawnedPawnsInFaction(allFaction).Count > 0)
				{
					AvoidGridMaker.RegenerateAvoidGridsFor(allFaction, map);
				}
			}
		}

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

		private static void PrintAvoidGridAroundTrapLoc(TrapMemory mem, ByteGrid avoidGrid)
		{
			Room room = mem.Cell.GetRoom(mem.map, RegionType.Set_Passable);
			for (int i = 0; i < AvoidGridMaker.TrapRadialCells; i++)
			{
				IntVec3 intVec = mem.Cell + GenRadial.RadialPattern[i];
				if (intVec.InBounds(mem.map) && intVec.Walkable(mem.map) && intVec.GetRoom(mem.map, RegionType.Set_Passable) == room)
				{
					float num = (float)Mathf.Max(1, intVec.DistanceToSquared(mem.Cell));
					int num2 = Mathf.Max(1, Mathf.RoundToInt((float)(32.0 * mem.PowerPercent / num)));
					AvoidGridMaker.IncrementAvoidGrid(avoidGrid, intVec, num2);
				}
			}
		}

		private static void PrintAvoidGridAroundTurret(Building_TurretGun tur, ByteGrid avoidGrid)
		{
			int num = GenRadial.NumCellsInRadius((float)(tur.GunCompEq.PrimaryVerb.verbProps.range + 4.0));
			for (int num2 = 0; num2 < num; num2++)
			{
				IntVec3 intVec = tur.Position + GenRadial.RadialPattern[num2];
				if (intVec.InBounds(tur.Map) && intVec.Walkable(tur.Map) && GenSight.LineOfSight(intVec, tur.Position, tur.Map, true, null, 0, 0))
				{
					AvoidGridMaker.IncrementAvoidGrid(avoidGrid, intVec, 12);
				}
			}
		}

		private static void IncrementAvoidGrid(ByteGrid avoidGrid, IntVec3 c, int num)
		{
			byte b = avoidGrid[c];
			b = (byte)Mathf.Min(255, b + num);
			avoidGrid[c] = b;
		}

		private static void ExpandAvoidGridIntoEdifices(ByteGrid avoidGrid, Map map)
		{
			int numGridCells = map.cellIndices.NumGridCells;
			for (int num = 0; num < numGridCells; num++)
			{
				if (avoidGrid[num] != 0 && map.edificeGrid[num] == null)
				{
					for (int i = 0; i < 8; i++)
					{
						IntVec3 c = map.cellIndices.IndexToCell(num) + GenAdj.AdjacentCells[i];
						if (c.InBounds(map))
						{
							Building edifice = c.GetEdifice(map);
							if (edifice != null)
							{
								avoidGrid[c] = (byte)Mathf.Min(255, Mathf.Max(avoidGrid[c], avoidGrid[num]));
							}
						}
					}
				}
			}
		}
	}
}
