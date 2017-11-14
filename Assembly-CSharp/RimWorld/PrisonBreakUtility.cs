using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public static class PrisonBreakUtility
	{
		private const float BaseInitiatePrisonBreakMtbDays = 45f;

		private const float DistanceToJoinPrisonBreak = 20f;

		private const float ChanceForRoomToJoinPrisonBreak = 0.5f;

		private const float SapperChance = 0.5f;

		private static readonly SimpleCurve PrisonBreakMTBFactorForDaysSincePrisonBreak = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(2f, 1.5f),
				true
			},
			{
				new CurvePoint(6f, 1f),
				true
			}
		};

		private static HashSet<Room> participatingRooms = new HashSet<Room>();

		private static List<Pawn> allEscapingPrisoners = new List<Pawn>();

		private static List<Room> tmpToRemove = new List<Room>();

		private static List<Pawn> escapingPrisonersGroup = new List<Pawn>();

		public static float InitiatePrisonBreakMtbDays(Pawn pawn)
		{
			if (!pawn.Awake())
			{
				return -1f;
			}
			if (!PrisonBreakUtility.CanParticipateInPrisonBreak(pawn))
			{
				return -1f;
			}
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room != null && room.isPrisonCell)
			{
				float num = 45f;
				num /= Mathf.Clamp(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving), 0.01f, 1f);
				if (pawn.guest.everParticipatedInPrisonBreak)
				{
					float x = (float)((float)(Find.TickManager.TicksGame - pawn.guest.lastPrisonBreakTicks) / 60000.0);
					num *= PrisonBreakUtility.PrisonBreakMTBFactorForDaysSincePrisonBreak.Evaluate(x);
				}
				return num;
			}
			return -1f;
		}

		public static bool CanParticipateInPrisonBreak(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return false;
			}
			if (!pawn.IsPrisoner)
			{
				return false;
			}
			if (PrisonBreakUtility.IsPrisonBreaking(pawn))
			{
				return false;
			}
			return true;
		}

		public static bool IsPrisonBreaking(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_PrisonBreak;
		}

		public static void StartPrisonBreak(Pawn initiator)
		{
			PrisonBreakUtility.participatingRooms.Clear();
			foreach (IntVec3 item in GenRadial.RadialCellsAround(initiator.Position, 20f, true))
			{
				if (item.InBounds(initiator.Map))
				{
					Room room = item.GetRoom(initiator.Map, RegionType.Set_Passable);
					if (room != null && room.isPrisonCell)
					{
						PrisonBreakUtility.participatingRooms.Add(room);
					}
				}
			}
			PrisonBreakUtility.RemoveRandomRooms(PrisonBreakUtility.participatingRooms, initiator);
			int sapperThingID = -1;
			if (Rand.Value < 0.5)
			{
				sapperThingID = initiator.thingIDNumber;
			}
			PrisonBreakUtility.allEscapingPrisoners.Clear();
			foreach (Room participatingRoom in PrisonBreakUtility.participatingRooms)
			{
				PrisonBreakUtility.StartPrisonBreakIn(participatingRoom, PrisonBreakUtility.allEscapingPrisoners, sapperThingID, PrisonBreakUtility.participatingRooms);
			}
			PrisonBreakUtility.participatingRooms.Clear();
			if (PrisonBreakUtility.allEscapingPrisoners.Any())
			{
				PrisonBreakUtility.SendPrisonBreakLetter(PrisonBreakUtility.allEscapingPrisoners);
			}
			PrisonBreakUtility.allEscapingPrisoners.Clear();
		}

		private static void RemoveRandomRooms(HashSet<Room> participatingRooms, Pawn initiator)
		{
			Room room = initiator.GetRoom(RegionType.Set_Passable);
			PrisonBreakUtility.tmpToRemove.Clear();
			foreach (Room participatingRoom in participatingRooms)
			{
				if (participatingRoom != room && !(Rand.Value < 0.5))
				{
					PrisonBreakUtility.tmpToRemove.Add(participatingRoom);
				}
			}
			for (int i = 0; i < PrisonBreakUtility.tmpToRemove.Count; i++)
			{
				participatingRooms.Remove(PrisonBreakUtility.tmpToRemove[i]);
			}
			PrisonBreakUtility.tmpToRemove.Clear();
		}

		private static void StartPrisonBreakIn(Room room, List<Pawn> outAllEscapingPrisoners, int sapperThingID, HashSet<Room> participatingRooms)
		{
			PrisonBreakUtility.escapingPrisonersGroup.Clear();
			PrisonBreakUtility.AddPrisonersFrom(room, PrisonBreakUtility.escapingPrisonersGroup);
			if (PrisonBreakUtility.escapingPrisonersGroup.Any())
			{
				foreach (Room participatingRoom in participatingRooms)
				{
					if (participatingRoom != room && PrisonBreakUtility.RoomsAreCloseToEachOther(room, participatingRoom))
					{
						PrisonBreakUtility.AddPrisonersFrom(participatingRoom, PrisonBreakUtility.escapingPrisonersGroup);
					}
				}
				IntVec3 exitPoint = default(IntVec3);
				IntVec3 groupUpLoc = default(IntVec3);
				if (RCellFinder.TryFindRandomExitSpot(PrisonBreakUtility.escapingPrisonersGroup[0], out exitPoint, TraverseMode.PassDoors) && PrisonBreakUtility.TryFindGroupUpLoc(PrisonBreakUtility.escapingPrisonersGroup, exitPoint, out groupUpLoc))
				{
					LordMaker.MakeNewLord(PrisonBreakUtility.escapingPrisonersGroup[0].Faction, new LordJob_PrisonBreak(groupUpLoc, exitPoint, sapperThingID), room.Map, PrisonBreakUtility.escapingPrisonersGroup);
					for (int i = 0; i < PrisonBreakUtility.escapingPrisonersGroup.Count; i++)
					{
						Pawn pawn = PrisonBreakUtility.escapingPrisonersGroup[i];
						if (pawn.CurJob != null && pawn.jobs.curDriver.layingDown != 0)
						{
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
						else
						{
							pawn.jobs.CheckForJobOverride();
						}
						pawn.guest.everParticipatedInPrisonBreak = true;
						pawn.guest.lastPrisonBreakTicks = Find.TickManager.TicksGame;
						outAllEscapingPrisoners.Add(pawn);
					}
					PrisonBreakUtility.escapingPrisonersGroup.Clear();
				}
			}
		}

		private static void AddPrisonersFrom(Room room, List<Pawn> outEscapingPrisoners)
		{
			foreach (Thing containedAndAdjacentThing in room.ContainedAndAdjacentThings)
			{
				Pawn pawn = containedAndAdjacentThing as Pawn;
				if (pawn != null && PrisonBreakUtility.CanParticipateInPrisonBreak(pawn))
				{
					outEscapingPrisoners.Add(pawn);
				}
			}
		}

		private static bool TryFindGroupUpLoc(List<Pawn> escapingPrisoners, IntVec3 exitPoint, out IntVec3 groupUpLoc)
		{
			groupUpLoc = IntVec3.Invalid;
			Map map = escapingPrisoners[0].Map;
			using (PawnPath pawnPath = map.pathFinder.FindPath(escapingPrisoners[0].Position, exitPoint, TraverseParms.For(escapingPrisoners[0], Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
			{
				if (!pawnPath.Found)
				{
					Log.Warning("Prison break: could not find path for prisoner " + escapingPrisoners[0] + " to the exit point.");
					return false;
				}
				for (int i = 0; i < pawnPath.NodesLeftCount; i++)
				{
					IntVec3 intVec = pawnPath.Peek(pawnPath.NodesLeftCount - i - 1);
					Room room = intVec.GetRoom(map, RegionType.Set_Passable);
					if (room != null && !room.isPrisonCell && (room.TouchesMapEdge || room.IsHuge || room.Cells.Count((IntVec3 x) => x.Standable(map)) >= 5))
					{
						groupUpLoc = CellFinder.RandomClosewalkCellNear(intVec, map, 3, null);
					}
				}
			}
			if (!groupUpLoc.IsValid)
			{
				groupUpLoc = escapingPrisoners[0].Position;
			}
			return true;
		}

		private static bool RoomsAreCloseToEachOther(Room a, Room b)
		{
			IntVec3 anyCell = a.Regions[0].AnyCell;
			IntVec3 anyCell2 = b.Regions[0].AnyCell;
			if (a.Map != b.Map)
			{
				return false;
			}
			if (!anyCell.WithinRegions(anyCell2, a.Map, 18, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false), RegionType.Set_Passable))
			{
				return false;
			}
			using (PawnPath pawnPath = a.Map.pathFinder.FindPath(anyCell, anyCell2, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false), PathEndMode.OnCell))
			{
				if (!pawnPath.Found)
				{
					return false;
				}
				return pawnPath.NodesLeftCount < 24;
			}
		}

		private static void SendPrisonBreakLetter(List<Pawn> escapingPrisoners)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < escapingPrisoners.Count; i++)
			{
				stringBuilder.AppendLine("    " + escapingPrisoners[i].LabelShort);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelPrisonBreak".Translate(), "LetterPrisonBreak".Translate(stringBuilder.ToString().TrimEndNewlines()), LetterDefOf.ThreatBig, PrisonBreakUtility.allEscapingPrisoners[0], null);
		}
	}
}
