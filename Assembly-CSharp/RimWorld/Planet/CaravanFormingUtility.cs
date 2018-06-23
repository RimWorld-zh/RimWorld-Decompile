using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020005DA RID: 1498
	[StaticConstructorOnStartup]
	public static class CaravanFormingUtility
	{
		// Token: 0x04001189 RID: 4489
		private static readonly Texture2D RemoveFromCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/RemoveFromCaravan", true);

		// Token: 0x0400118A RID: 4490
		private static readonly Texture2D AddToCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/AddToCaravan", true);

		// Token: 0x0400118B RID: 4491
		private static List<Thing> tmpReachableItems = new List<Thing>();

		// Token: 0x0400118C RID: 4492
		private static List<Pawn> tmpSendablePawns = new List<Pawn>();

		// Token: 0x0400118D RID: 4493
		private static List<ThingCount> tmpCaravanPawns = new List<ThingCount>();

		// Token: 0x06001D84 RID: 7556 RVA: 0x000FE4C2 File Offset: 0x000FC8C2
		public static void FormAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, int directionTile, int destinationTile)
		{
			CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, true);
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000FE4D4 File Offset: 0x000FC8D4
		public static void StartFormingCaravan(List<Pawn> pawns, Faction faction, List<TransferableOneWay> transferables, IntVec3 meetingPoint, IntVec3 exitSpot, int startingTile, int destinationTile)
		{
			if (startingTile < 0)
			{
				Log.Error("Can't start forming caravan because startingTile is invalid.", false);
			}
			else if (!pawns.Any<Pawn>())
			{
				Log.Error("Can't start forming caravan with 0 pawns.", false);
			}
			else
			{
				if (pawns.Any((Pawn x) => x.Downed))
				{
					Log.Warning("Forming a caravan with a downed pawn. This shouldn't happen because we have to create a Lord.", false);
				}
				List<TransferableOneWay> list = transferables.ToList<TransferableOneWay>();
				list.RemoveAll((TransferableOneWay x) => x.CountToTransfer <= 0 || !x.HasAnyThing || x.AnyThing is Pawn);
				for (int i = 0; i < pawns.Count; i++)
				{
					Lord lord = pawns[i].GetLord();
					if (lord != null)
					{
						lord.Notify_PawnLost(pawns[i], PawnLostCondition.ForcedToJoinOtherLord);
					}
				}
				LordJob_FormAndSendCaravan lordJob = new LordJob_FormAndSendCaravan(list, meetingPoint, exitSpot, startingTile, destinationTile);
				LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, pawns[0].MapHeld, pawns);
				for (int j = 0; j < pawns.Count; j++)
				{
					Pawn pawn = pawns[j];
					if (pawn.Spawned)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
					}
				}
			}
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x000FE617 File Offset: 0x000FCA17
		public static void StopFormingCaravan(Lord lord)
		{
			CaravanFormingUtility.SetToUnloadEverything(lord);
			lord.lordManager.RemoveLord(lord);
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000FE62C File Offset: 0x000FCA2C
		public static void RemovePawnFromCaravan(Pawn pawn, Lord lord)
		{
			bool flag = false;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn2 = lord.ownedPawns[i];
				if (pawn2 != pawn && CaravanUtility.IsOwner(pawn2, Faction.OfPlayer))
				{
					flag = true;
					break;
				}
			}
			bool flag2 = true;
			string text = "MessagePawnLostWhileFormingCaravan".Translate(new object[]
			{
				pawn.LabelIndefinite()
			}).CapitalizeFirst();
			if (!flag)
			{
				CaravanFormingUtility.StopFormingCaravan(lord);
				text = text + " " + "MessagePawnLostWhileFormingCaravan_AllLost".Translate();
			}
			else
			{
				pawn.inventory.UnloadEverything = true;
				if (lord.ownedPawns.Contains(pawn))
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedByPlayerAction);
					flag2 = false;
				}
			}
			if (flag2)
			{
				Messages.Message(text, pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000FE718 File Offset: 0x000FCB18
		public static void Notify_FormAndSendCaravanLordFailed(Lord lord)
		{
			CaravanFormingUtility.SetToUnloadEverything(lord);
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000FE724 File Offset: 0x000FCB24
		private static void SetToUnloadEverything(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				lord.ownedPawns[i].inventory.UnloadEverything = true;
			}
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000FE768 File Offset: 0x000FCB68
		public static List<Thing> AllReachableColonyItems(Map map, bool allowEvenIfOutsideHomeArea = false, bool allowEvenIfReserved = false, bool canMinify = false)
		{
			CaravanFormingUtility.tmpReachableItems.Clear();
			List<Thing> allThings = map.listerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				bool flag = canMinify && thing.def.Minifiable;
				if ((flag || thing.def.category == ThingCategory.Item) && (allowEvenIfOutsideHomeArea || map.areaManager.Home[thing.Position] || thing.IsInAnyStorage()) && (!thing.Position.Fogged(thing.Map) && (allowEvenIfReserved || !map.reservationManager.IsReservedByAnyoneOf(thing, Faction.OfPlayer))) && (flag || thing.def.EverHaulable))
				{
					CaravanFormingUtility.tmpReachableItems.Add(thing);
				}
			}
			return CaravanFormingUtility.tmpReachableItems;
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x000FE874 File Offset: 0x000FCC74
		public static List<Pawn> AllSendablePawns(Map map, bool allowEvenIfDownedOrInMentalState = false, bool allowEvenIfPrisonerNotSecure = false, bool allowCapturableDownedPawns = false)
		{
			CaravanFormingUtility.tmpSendablePawns.Clear();
			List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if (allowEvenIfDownedOrInMentalState || (!pawn.Downed && !pawn.InMentalState))
				{
					if (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || (allowCapturableDownedPawns && pawn.Downed && CaravanUtility.ShouldAutoCapture(pawn, Faction.OfPlayer)))
					{
						if ((allowEvenIfPrisonerNotSecure || !pawn.IsPrisoner || pawn.guest.PrisonerIsSecure) && (pawn.GetLord() == null || pawn.GetLord().LordJob is LordJob_VoluntarilyJoinable))
						{
							CaravanFormingUtility.tmpSendablePawns.Add(pawn);
						}
					}
				}
			}
			return CaravanFormingUtility.tmpSendablePawns;
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x000FE97C File Offset: 0x000FCD7C
		public static IEnumerable<Gizmo> GetGizmos(Pawn pawn)
		{
			if (pawn.IsFormingCaravan())
			{
				Lord lord = pawn.GetLord();
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelFormingCaravan".Translate(),
					defaultDesc = "CommandCancelFormingCaravanDesc".Translate(),
					icon = TexCommand.ClearPrioritizedWork,
					activateSound = SoundDefOf.Tick_Low,
					action = delegate()
					{
						CaravanFormingUtility.StopFormingCaravan(lord);
					},
					hotKey = KeyBindingDefOf.Designator_Cancel
				};
				yield return new Command_Action
				{
					defaultLabel = "CommandRemoveFromCaravan".Translate(),
					defaultDesc = "CommandRemoveFromCaravanDesc".Translate(),
					icon = CaravanFormingUtility.RemoveFromCaravanCommand,
					action = delegate()
					{
						CaravanFormingUtility.RemovePawnFromCaravan(pawn, lord);
					},
					hotKey = KeyBindingDefOf.Misc6
				};
			}
			else if (Dialog_FormCaravan.AllSendablePawns(pawn.Map, false).Contains(pawn))
			{
				bool anyCaravanToJoin = false;
				for (int i = 0; i < pawn.Map.lordManager.lords.Count; i++)
				{
					Lord lord2 = pawn.Map.lordManager.lords[i];
					if (lord2.faction == Faction.OfPlayer && lord2.LordJob is LordJob_FormAndSendCaravan)
					{
						anyCaravanToJoin = true;
						break;
					}
				}
				if (anyCaravanToJoin)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandAddToCaravan".Translate(),
						defaultDesc = "CommandAddToCaravanDesc".Translate(),
						icon = CaravanFormingUtility.AddToCaravanCommand,
						action = delegate()
						{
							List<Lord> list = new List<Lord>();
							for (int j = 0; j < pawn.Map.lordManager.lords.Count; j++)
							{
								Lord lord3 = pawn.Map.lordManager.lords[j];
								if (lord3.faction == Faction.OfPlayer && lord3.LordJob is LordJob_FormAndSendCaravan)
								{
									list.Add(lord3);
								}
							}
							if (list.Count != 0)
							{
								if (list.Count == 1)
								{
									CaravanFormingUtility.LateJoinFormingCaravan(pawn, list[0]);
									SoundDefOf.Click.PlayOneShotOnCamera(null);
								}
								else
								{
									List<FloatMenuOption> list2 = new List<FloatMenuOption>();
									for (int k = 0; k < list.Count; k++)
									{
										Lord caravanLocal = list[k];
										string label = "Caravan".Translate() + " " + (k + 1);
										list2.Add(new FloatMenuOption(label, delegate()
										{
											if (pawn.Spawned && pawn.Map.lordManager.lords.Contains(caravanLocal) && Dialog_FormCaravan.AllSendablePawns(pawn.Map, false).Contains(pawn))
											{
												CaravanFormingUtility.LateJoinFormingCaravan(pawn, caravanLocal);
											}
										}, MenuOptionPriority.Default, null, null, 0f, null, null));
									}
									Find.WindowStack.Add(new FloatMenu(list2));
								}
							}
						},
						hotKey = KeyBindingDefOf.Misc7
					};
				}
			}
			yield break;
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000FE9A8 File Offset: 0x000FCDA8
		private static void LateJoinFormingCaravan(Pawn pawn, Lord lord)
		{
			Lord lord2 = pawn.GetLord();
			if (lord2 != null)
			{
				lord2.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord);
			}
			lord.AddPawn(pawn);
			if (pawn.Spawned)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
			}
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x000FE9EC File Offset: 0x000FCDEC
		public static bool IsFormingCaravan(this Pawn p)
		{
			Lord lord = p.GetLord();
			return lord != null && lord.LordJob is LordJob_FormAndSendCaravan;
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x000FEA20 File Offset: 0x000FCE20
		public static float CapacityLeft(LordJob_FormAndSendCaravan lordJob)
		{
			float num = CollectionsMassCalculator.MassUsageTransferables(lordJob.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			for (int i = 0; i < lordJob.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lordJob.lord.ownedPawns[i];
				CaravanFormingUtility.tmpCaravanPawns.Add(new ThingCount(pawn, pawn.stackCount));
			}
			num += CollectionsMassCalculator.MassUsage(CaravanFormingUtility.tmpCaravanPawns, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			float num2 = CollectionsMassCalculator.Capacity(CaravanFormingUtility.tmpCaravanPawns, null);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			return num2 - num;
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x000FEAC4 File Offset: 0x000FCEC4
		public static string AppendOverweightInfo(string text, float capacityLeft)
		{
			if (capacityLeft < 0f)
			{
				text = text + " (" + "OverweightLower".Translate() + ")";
			}
			return text;
		}
	}
}
