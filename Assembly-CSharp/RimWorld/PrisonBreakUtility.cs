using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020004EF RID: 1263
	public static class PrisonBreakUtility
	{
		// Token: 0x0600169E RID: 5790 RVA: 0x000C8444 File Offset: 0x000C6844
		public static float InitiatePrisonBreakMtbDays(Pawn pawn)
		{
			float result;
			if (!pawn.Awake())
			{
				result = -1f;
			}
			else if (!PrisonBreakUtility.CanParticipateInPrisonBreak(pawn))
			{
				result = -1f;
			}
			else
			{
				Room room = pawn.GetRoom(RegionType.Set_Passable);
				if (room == null || !room.isPrisonCell)
				{
					result = -1f;
				}
				else
				{
					float num = 45f;
					num /= Mathf.Clamp(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving), 0.01f, 1f);
					if (pawn.guest.everParticipatedInPrisonBreak)
					{
						float x = (float)(Find.TickManager.TicksGame - pawn.guest.lastPrisonBreakTicks) / 60000f;
						num *= PrisonBreakUtility.PrisonBreakMTBFactorForDaysSincePrisonBreak.Evaluate(x);
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x000C8518 File Offset: 0x000C6918
		public static bool CanParticipateInPrisonBreak(Pawn pawn)
		{
			return !pawn.Downed && pawn.IsPrisoner && !PrisonBreakUtility.IsPrisonBreaking(pawn);
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x000C8564 File Offset: 0x000C6964
		public static bool IsPrisonBreaking(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_PrisonBreak;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x000C8598 File Offset: 0x000C6998
		public static void StartPrisonBreak(Pawn initiator)
		{
			string text;
			string label;
			LetterDef textLetterDef;
			PrisonBreakUtility.StartPrisonBreak(initiator, out text, out label, out textLetterDef);
			if (!text.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(label, text, textLetterDef, initiator, null, null);
			}
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x000C85D4 File Offset: 0x000C69D4
		public static void StartPrisonBreak(Pawn initiator, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			PrisonBreakUtility.participatingRooms.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(initiator.Position, 20f, true))
			{
				if (intVec.InBounds(initiator.Map))
				{
					Room room = intVec.GetRoom(initiator.Map, RegionType.Set_Passable);
					if (room != null && room.isPrisonCell)
					{
						PrisonBreakUtility.participatingRooms.Add(room);
					}
				}
			}
			PrisonBreakUtility.RemoveRandomRooms(PrisonBreakUtility.participatingRooms, initiator);
			int sapperThingID = -1;
			if (Rand.Value < 0.5f)
			{
				sapperThingID = initiator.thingIDNumber;
			}
			PrisonBreakUtility.allEscapingPrisoners.Clear();
			foreach (Room room2 in PrisonBreakUtility.participatingRooms)
			{
				PrisonBreakUtility.StartPrisonBreakIn(room2, PrisonBreakUtility.allEscapingPrisoners, sapperThingID, PrisonBreakUtility.participatingRooms);
			}
			PrisonBreakUtility.participatingRooms.Clear();
			if (PrisonBreakUtility.allEscapingPrisoners.Any<Pawn>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < PrisonBreakUtility.allEscapingPrisoners.Count; i++)
				{
					stringBuilder.AppendLine("    " + PrisonBreakUtility.allEscapingPrisoners[i].LabelShort);
				}
				letterText = "LetterPrisonBreak".Translate(new object[]
				{
					stringBuilder.ToString().TrimEndNewlines()
				});
				letterLabel = "LetterLabelPrisonBreak".Translate();
				letterDef = LetterDefOf.ThreatBig;
				PrisonBreakUtility.allEscapingPrisoners.Clear();
			}
			else
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
			}
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x000C87BC File Offset: 0x000C6BBC
		private static void RemoveRandomRooms(HashSet<Room> participatingRooms, Pawn initiator)
		{
			Room room = initiator.GetRoom(RegionType.Set_Passable);
			PrisonBreakUtility.tmpToRemove.Clear();
			foreach (Room room2 in participatingRooms)
			{
				if (room2 != room)
				{
					if (Rand.Value >= 0.5f)
					{
						PrisonBreakUtility.tmpToRemove.Add(room2);
					}
				}
			}
			for (int i = 0; i < PrisonBreakUtility.tmpToRemove.Count; i++)
			{
				participatingRooms.Remove(PrisonBreakUtility.tmpToRemove[i]);
			}
			PrisonBreakUtility.tmpToRemove.Clear();
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x000C8888 File Offset: 0x000C6C88
		private static void StartPrisonBreakIn(Room room, List<Pawn> outAllEscapingPrisoners, int sapperThingID, HashSet<Room> participatingRooms)
		{
			PrisonBreakUtility.escapingPrisonersGroup.Clear();
			PrisonBreakUtility.AddPrisonersFrom(room, PrisonBreakUtility.escapingPrisonersGroup);
			if (PrisonBreakUtility.escapingPrisonersGroup.Any<Pawn>())
			{
				foreach (Room room2 in participatingRooms)
				{
					if (room2 != room)
					{
						if (PrisonBreakUtility.RoomsAreCloseToEachOther(room, room2))
						{
							PrisonBreakUtility.AddPrisonersFrom(room2, PrisonBreakUtility.escapingPrisonersGroup);
						}
					}
				}
				IntVec3 exitPoint;
				if (RCellFinder.TryFindRandomExitSpot(PrisonBreakUtility.escapingPrisonersGroup[0], out exitPoint, TraverseMode.PassDoors))
				{
					IntVec3 groupUpLoc;
					if (PrisonBreakUtility.TryFindGroupUpLoc(PrisonBreakUtility.escapingPrisonersGroup, exitPoint, out groupUpLoc))
					{
						LordMaker.MakeNewLord(PrisonBreakUtility.escapingPrisonersGroup[0].Faction, new LordJob_PrisonBreak(groupUpLoc, exitPoint, sapperThingID), room.Map, PrisonBreakUtility.escapingPrisonersGroup);
						for (int i = 0; i < PrisonBreakUtility.escapingPrisonersGroup.Count; i++)
						{
							Pawn pawn = PrisonBreakUtility.escapingPrisonersGroup[i];
							if (pawn.CurJob != null && pawn.GetPosture().Laying())
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
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x000C8A2C File Offset: 0x000C6E2C
		private static void AddPrisonersFrom(Room room, List<Pawn> outEscapingPrisoners)
		{
			foreach (Thing thing in room.ContainedAndAdjacentThings)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && PrisonBreakUtility.CanParticipateInPrisonBreak(pawn))
				{
					outEscapingPrisoners.Add(pawn);
				}
			}
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x000C8AAC File Offset: 0x000C6EAC
		private static bool TryFindGroupUpLoc(List<Pawn> escapingPrisoners, IntVec3 exitPoint, out IntVec3 groupUpLoc)
		{
			groupUpLoc = IntVec3.Invalid;
			Map map = escapingPrisoners[0].Map;
			using (PawnPath pawnPath = map.pathFinder.FindPath(escapingPrisoners[0].Position, exitPoint, TraverseParms.For(escapingPrisoners[0], Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
			{
				if (!pawnPath.Found)
				{
					Log.Warning("Prison break: could not find path for prisoner " + escapingPrisoners[0] + " to the exit point.", false);
					return false;
				}
				for (int i = 0; i < pawnPath.NodesLeftCount; i++)
				{
					IntVec3 intVec = pawnPath.Peek(pawnPath.NodesLeftCount - i - 1);
					Room room = intVec.GetRoom(map, RegionType.Set_Passable);
					if (room != null)
					{
						if (!room.isPrisonCell)
						{
							if (room.TouchesMapEdge || room.IsHuge || room.Cells.Count((IntVec3 x) => x.Standable(map)) >= 5)
							{
								groupUpLoc = CellFinder.RandomClosewalkCellNear(intVec, map, 3, null);
							}
						}
					}
				}
			}
			if (!groupUpLoc.IsValid)
			{
				groupUpLoc = escapingPrisoners[0].Position;
			}
			return true;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x000C8C30 File Offset: 0x000C7030
		private static bool RoomsAreCloseToEachOther(Room a, Room b)
		{
			IntVec3 anyCell = a.Regions[0].AnyCell;
			IntVec3 anyCell2 = b.Regions[0].AnyCell;
			bool result;
			if (a.Map != b.Map)
			{
				result = false;
			}
			else if (!anyCell.WithinRegions(anyCell2, a.Map, 18, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false), RegionType.Set_Passable))
			{
				result = false;
			}
			else
			{
				using (PawnPath pawnPath = a.Map.pathFinder.FindPath(anyCell, anyCell2, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false), PathEndMode.OnCell))
				{
					if (!pawnPath.Found)
					{
						result = false;
					}
					else
					{
						result = (pawnPath.NodesLeftCount < 24);
					}
				}
			}
			return result;
		}

		// Token: 0x04000D32 RID: 3378
		private const float BaseInitiatePrisonBreakMtbDays = 45f;

		// Token: 0x04000D33 RID: 3379
		private const float DistanceToJoinPrisonBreak = 20f;

		// Token: 0x04000D34 RID: 3380
		private const float ChanceForRoomToJoinPrisonBreak = 0.5f;

		// Token: 0x04000D35 RID: 3381
		private const float SapperChance = 0.5f;

		// Token: 0x04000D36 RID: 3382
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

		// Token: 0x04000D37 RID: 3383
		private static HashSet<Room> participatingRooms = new HashSet<Room>();

		// Token: 0x04000D38 RID: 3384
		private static List<Pawn> allEscapingPrisoners = new List<Pawn>();

		// Token: 0x04000D39 RID: 3385
		private static List<Room> tmpToRemove = new List<Room>();

		// Token: 0x04000D3A RID: 3386
		private static List<Pawn> escapingPrisonersGroup = new List<Pawn>();
	}
}
