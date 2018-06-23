using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200017D RID: 381
	public static class PartyUtility
	{
		// Token: 0x0400036D RID: 877
		private const float PartyAreaRadiusIfNotWholeRoom = 10f;

		// Token: 0x0400036E RID: 878
		private const int MaxRoomCellsCountToUseWholeRoom = 324;

		// Token: 0x060007EF RID: 2031 RVA: 0x0004D674 File Offset: 0x0004BA74
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
			else if (map.dangerWatcher.DangerRating != StoryDanger.None)
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
					foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
					{
						if (pawn.health.hediffSet.BleedRateTotal > 0f)
						{
							return false;
						}
						if (pawn.Drafted)
						{
							num++;
						}
					}
					result = ((float)num / (float)freeColonistsSpawnedCount < 0.5f && PartyUtility.EnoughPotentialGuestsToStartParty(map, null));
				}
			}
			return result;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0004D7B8 File Offset: 0x0004BBB8
		public static bool AcceptableGameConditionsToContinueParty(Map map)
		{
			return map.dangerWatcher.DangerRating != StoryDanger.High;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0004D7E8 File Offset: 0x0004BBE8
		public static bool EnoughPotentialGuestsToStartParty(Map map, IntVec3? partySpot = null)
		{
			int num = Mathf.RoundToInt((float)map.mapPawns.FreeColonistsSpawnedCount * 0.65f);
			num = Mathf.Clamp(num, 2, 10);
			int num2 = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (PartyUtility.ShouldPawnKeepPartying(pawn))
				{
					if (partySpot == null || !partySpot.Value.IsForbidden(pawn))
					{
						if (partySpot == null || pawn.CanReach(partySpot.Value, PathEndMode.Touch, Danger.Some, false, TraverseMode.ByPawn))
						{
							num2++;
						}
					}
				}
			}
			return num2 >= num;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0004D8DC File Offset: 0x0004BCDC
		public static Pawn FindRandomPartyOrganizer(Faction faction, Map map)
		{
			Predicate<Pawn> validator = (Pawn x) => x.RaceProps.Humanlike && !x.InBed() && !x.InMentalState && x.GetLord() == null && PartyUtility.ShouldPawnKeepPartying(x);
			Pawn pawn;
			Pawn result;
			if ((from x in map.mapPawns.SpawnedPawnsInFaction(faction)
			where validator(x)
			select x).TryRandomElement(out pawn))
			{
				result = pawn;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0004D94C File Offset: 0x0004BD4C
		public static bool ShouldPawnKeepPartying(Pawn p)
		{
			return (p.timetable == null || p.timetable.CurrentAssignment.allowJoy) && GatheringsUtility.ShouldGuestKeepAttendingGathering(p);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0004D99C File Offset: 0x0004BD9C
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
				if (edifice != null)
				{
					result = map.reachability.CanReach(partySpot, edifice, PathEndMode.ClosestTouch, traverseParams);
				}
				else
				{
					result = map.reachability.CanReach(partySpot, cell, PathEndMode.ClosestTouch, traverseParams);
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0004DA38 File Offset: 0x0004BE38
		public static bool TryFindRandomCellInPartyArea(Pawn pawn, out IntVec3 result)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Predicate<IntVec3> validator = (IntVec3 x) => x.Standable(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.None, 1, -1, null, false);
			bool result2;
			if (PartyUtility.UseWholeRoomAsPartyArea(cell, pawn.Map))
			{
				Room room = cell.GetRoom(pawn.Map, RegionType.Set_Passable);
				result2 = (from x in room.Cells
				where validator(x)
				select x).TryRandomElement(out result);
			}
			else
			{
				result2 = CellFinder.TryFindRandomReachableCellNear(cell, pawn.Map, 10f, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 x) => validator(x), null, out result, 10);
			}
			return result2;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0004DB04 File Offset: 0x0004BF04
		public static bool UseWholeRoomAsPartyArea(IntVec3 partySpot, Map map)
		{
			Room room = partySpot.GetRoom(map, RegionType.Set_Passable);
			return room != null && !room.IsHuge && !room.PsychologicallyOutdoors && room.CellCount <= 324;
		}
	}
}
