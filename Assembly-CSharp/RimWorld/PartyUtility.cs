using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class PartyUtility
	{
		private const float PartyAreaRadiusIfNotWholeRoom = 10f;

		private const int MaxRoomCellsCountToUseWholeRoom = 324;

		public static bool AcceptableGameConditionsToStartParty(Map map)
		{
			if (!PartyUtility.AcceptableGameConditionsToContinueParty(map))
			{
				return false;
			}
			if (GenLocalDate.HourInteger(map) >= 4 && GenLocalDate.HourInteger(map) <= 21)
			{
				if (GatheringsUtility.AnyLordJobPreventsNewGatherings(map))
				{
					return false;
				}
				if (map.dangerWatcher.DangerRating != 0)
				{
					return false;
				}
				int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
				if (freeColonistsSpawnedCount < 4)
				{
					return false;
				}
				int num = 0;
				foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
				{
					if (item.health.hediffSet.BleedRateTotal > 0.0)
					{
						return false;
					}
					if (item.Drafted)
					{
						num++;
					}
				}
				if ((float)num / (float)freeColonistsSpawnedCount >= 0.5)
				{
					return false;
				}
				int value = Mathf.RoundToInt((float)((float)map.mapPawns.FreeColonistsSpawnedCount * 0.64999997615814209));
				value = Mathf.Clamp(value, 2, 10);
				int num2 = 0;
				foreach (Pawn item2 in map.mapPawns.FreeColonistsSpawned)
				{
					if (PartyUtility.ShouldPawnKeepPartying(item2))
					{
						num2++;
					}
				}
				if (num2 < value)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool AcceptableGameConditionsToContinueParty(Map map)
		{
			if (map.dangerWatcher.DangerRating == StoryDanger.High)
			{
				return false;
			}
			return true;
		}

		public static Pawn FindRandomPartyOrganizer(Faction faction, Map map)
		{
			Predicate<Pawn> validator = (Predicate<Pawn>)((Pawn x) => x.RaceProps.Humanlike && !x.InBed() && PartyUtility.ShouldPawnKeepPartying(x));
			Pawn result = default(Pawn);
			if ((from x in map.mapPawns.SpawnedPawnsInFaction(faction)
			where validator(x)
			select x).TryRandomElement<Pawn>(out result))
			{
				return result;
			}
			return null;
		}

		public static bool ShouldPawnKeepPartying(Pawn p)
		{
			if (p.timetable != null && !p.timetable.CurrentAssignment.allowJoy)
			{
				return false;
			}
			if (!GatheringsUtility.ShouldGuestKeepAttendingGathering(p))
			{
				return false;
			}
			return true;
		}

		public static bool InPartyArea(IntVec3 cell, IntVec3 partySpot, Map map)
		{
			if (PartyUtility.UseWholeRoomAsPartyArea(partySpot, map) && cell.GetRoom(map, RegionType.Set_Passable) == partySpot.GetRoom(map, RegionType.Set_Passable))
			{
				return true;
			}
			if (cell.InHorDistOf(partySpot, 10f))
			{
				Building edifice = cell.GetEdifice(map);
				TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.None, false);
				if (edifice != null)
				{
					return map.reachability.CanReach(partySpot, (Thing)edifice, PathEndMode.ClosestTouch, traverseParams);
				}
				return map.reachability.CanReach(partySpot, cell, PathEndMode.ClosestTouch, traverseParams);
			}
			return false;
		}

		public static bool TryFindRandomCellInPartyArea(Pawn pawn, out IntVec3 result)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Predicate<IntVec3> validator = (Predicate<IntVec3>)((IntVec3 x) => x.Standable(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.None, 1, -1, null, false));
			if (PartyUtility.UseWholeRoomAsPartyArea(cell, pawn.Map))
			{
				Room room = cell.GetRoom(pawn.Map, RegionType.Set_Passable);
				return (from x in room.Cells
				where validator(x)
				select x).TryRandomElement<IntVec3>(out result);
			}
			return CellFinder.TryFindRandomReachableCellNear(cell, pawn.Map, 10f, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (Predicate<IntVec3>)((IntVec3 x) => validator(x)), (Predicate<Region>)null, out result, 10);
		}

		public static bool UseWholeRoomAsPartyArea(IntVec3 partySpot, Map map)
		{
			Room room = partySpot.GetRoom(map, RegionType.Set_Passable);
			if (room != null && !room.IsHuge && !room.PsychologicallyOutdoors && room.CellCount <= 324)
			{
				return true;
			}
			return false;
		}
	}
}
