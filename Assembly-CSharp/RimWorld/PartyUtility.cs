using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public static class PartyUtility
	{
		private const float PartyAreaRadiusIfNotWholeRoom = 10f;

		private const int MaxRoomCellsCountToUseWholeRoom = 324;

		public static bool AcceptableGameConditionsToStartParty(Map map)
		{
			bool result;
			if (!PartyUtility.AcceptableGameConditionsToContinueParty(map))
			{
				result = false;
			}
			else if (GenLocalDate.HourInteger(map) < 4 || GenLocalDate.HourInteger(map) > 21)
			{
				result = false;
			}
			else if (GatheringsUtility.AnyLordJobPreventsNewGatherings(map))
			{
				result = false;
			}
			else if (map.dangerWatcher.DangerRating != 0)
			{
				result = false;
			}
			else
			{
				int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
				if (freeColonistsSpawnedCount < 4)
				{
					result = false;
				}
				else
				{
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
						result = false;
					}
					else
					{
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
						result = ((byte)((num2 >= value) ? 1 : 0) != 0);
					}
				}
			}
			return result;
		}

		public static bool AcceptableGameConditionsToContinueParty(Map map)
		{
			return (byte)((map.dangerWatcher.DangerRating != StoryDanger.High) ? 1 : 0) != 0;
		}

		public static Pawn FindRandomPartyOrganizer(Faction faction, Map map)
		{
			Predicate<Pawn> validator = (Predicate<Pawn>)((Pawn x) => x.RaceProps.Humanlike && !x.InBed() && !x.InMentalState && x.GetLord() == null && PartyUtility.ShouldPawnKeepPartying(x));
			Pawn pawn = default(Pawn);
			return (!(from x in map.mapPawns.SpawnedPawnsInFaction(faction)
			where validator(x)
			select x).TryRandomElement<Pawn>(out pawn)) ? null : pawn;
		}

		public static bool ShouldPawnKeepPartying(Pawn p)
		{
			return (byte)((p.timetable == null || p.timetable.CurrentAssignment.allowJoy) ? (GatheringsUtility.ShouldGuestKeepAttendingGathering(p) ? 1 : 0) : 0) != 0;
		}

		public static bool InPartyArea(IntVec3 cell, IntVec3 partySpot, Map map)
		{
			bool result;
			if (PartyUtility.UseWholeRoomAsPartyArea(partySpot, map) && cell.GetRoom(map, RegionType.Set_Passable) == partySpot.GetRoom(map, RegionType.Set_Passable))
			{
				result = true;
			}
			else if (cell.InHorDistOf(partySpot, 10f))
			{
				Building edifice = cell.GetEdifice(map);
				TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.None, false);
				result = ((edifice == null) ? map.reachability.CanReach(partySpot, cell, PathEndMode.ClosestTouch, traverseParams) : map.reachability.CanReach(partySpot, (Thing)edifice, PathEndMode.ClosestTouch, traverseParams));
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static bool TryFindRandomCellInPartyArea(Pawn pawn, out IntVec3 result)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Predicate<IntVec3> validator = (Predicate<IntVec3>)((IntVec3 x) => x.Standable(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.None, 1, -1, null, false));
			bool result2;
			if (PartyUtility.UseWholeRoomAsPartyArea(cell, pawn.Map))
			{
				Room room = cell.GetRoom(pawn.Map, RegionType.Set_Passable);
				result2 = (from x in room.Cells
				where validator(x)
				select x).TryRandomElement<IntVec3>(out result);
			}
			else
			{
				result2 = CellFinder.TryFindRandomReachableCellNear(cell, pawn.Map, 10f, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (Predicate<IntVec3>)((IntVec3 x) => validator(x)), (Predicate<Region>)null, out result, 10);
			}
			return result2;
		}

		public static bool UseWholeRoomAsPartyArea(IntVec3 partySpot, Map map)
		{
			Room room = partySpot.GetRoom(map, RegionType.Set_Passable);
			return (byte)((room != null && !room.IsHuge && !room.PsychologicallyOutdoors && room.CellCount <= 324) ? 1 : 0) != 0;
		}
	}
}
