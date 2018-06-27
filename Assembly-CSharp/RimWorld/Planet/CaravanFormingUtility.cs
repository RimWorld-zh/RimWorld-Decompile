using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public static class CaravanFormingUtility
	{
		private static readonly Texture2D RemoveFromCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/RemoveFromCaravan", true);

		private static readonly Texture2D AddToCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/AddToCaravan", true);

		private static List<ThingCount> tmpCaravanPawns = new List<ThingCount>();

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<TransferableOneWay> <>f__am$cache1;

		public static void FormAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, int directionTile, int destinationTile)
		{
			CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, true);
		}

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

		public static void StopFormingCaravan(Lord lord)
		{
			CaravanFormingUtility.SetToUnloadEverything(lord);
			lord.lordManager.RemoveLord(lord);
		}

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

		public static void Notify_FormAndSendCaravanLordFailed(Lord lord)
		{
			CaravanFormingUtility.SetToUnloadEverything(lord);
		}

		private static void SetToUnloadEverything(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				lord.ownedPawns[i].inventory.UnloadEverything = true;
			}
		}

		public static List<Thing> AllReachableColonyItems(Map map, bool allowEvenIfOutsideHomeArea = false, bool allowEvenIfReserved = false, bool canMinify = false)
		{
			List<Thing> list = new List<Thing>();
			List<Thing> allThings = map.listerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				bool flag = canMinify && thing.def.Minifiable;
				if ((flag || thing.def.category == ThingCategory.Item) && (allowEvenIfOutsideHomeArea || map.areaManager.Home[thing.Position] || thing.IsInAnyStorage()) && (!thing.Position.Fogged(thing.Map) && (allowEvenIfReserved || !map.reservationManager.IsReservedByAnyoneOf(thing, Faction.OfPlayer))) && (flag || thing.def.EverHaulable))
				{
					list.Add(thing);
				}
			}
			return list;
		}

		public static List<Pawn> AllSendablePawns(Map map, bool allowEvenIfDownedOrInMentalState = false, bool allowEvenIfPrisonerNotSecure = false, bool allowCapturableDownedPawns = false)
		{
			List<Pawn> list = new List<Pawn>();
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
							list.Add(pawn);
						}
					}
				}
			}
			return list;
		}

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
			else if (pawn.Spawned)
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
				if (anyCaravanToJoin && Dialog_FormCaravan.AllSendablePawns(pawn.Map, false).Contains(pawn))
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

		public static bool IsFormingCaravan(this Pawn p)
		{
			Lord lord = p.GetLord();
			return lord != null && lord.LordJob is LordJob_FormAndSendCaravan;
		}

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

		public static string AppendOverweightInfo(string text, float capacityLeft)
		{
			if (capacityLeft < 0f)
			{
				text = text + " (" + "OverweightLower".Translate() + ")";
			}
			return text;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CaravanFormingUtility()
		{
		}

		[CompilerGenerated]
		private static bool <StartFormingCaravan>m__0(Pawn x)
		{
			return x.Downed;
		}

		[CompilerGenerated]
		private static bool <StartFormingCaravan>m__1(TransferableOneWay x)
		{
			return x.CountToTransfer <= 0 || !x.HasAnyThing || x.AnyThing is Pawn;
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Pawn pawn;

			internal Command_Action <cancelCaravan>__2;

			internal Command_Action <removeFromCaravan>__3;

			internal bool <anyCaravanToJoin>__4;

			internal Command_Action <addToCaravan>__5;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private CaravanFormingUtility.<GetGizmos>c__Iterator0.<GetGizmos>c__AnonStorey2 $locvar0;

			private CaravanFormingUtility.<GetGizmos>c__Iterator0.<GetGizmos>c__AnonStorey1 $locvar1;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (pawn.IsFormingCaravan())
					{
						Lord lord = pawn.GetLord();
						Command_Action cancelCaravan = new Command_Action();
						cancelCaravan.defaultLabel = "CommandCancelFormingCaravan".Translate();
						cancelCaravan.defaultDesc = "CommandCancelFormingCaravanDesc".Translate();
						cancelCaravan.icon = TexCommand.ClearPrioritizedWork;
						cancelCaravan.activateSound = SoundDefOf.Tick_Low;
						cancelCaravan.action = delegate()
						{
							CaravanFormingUtility.StopFormingCaravan(lord);
						};
						cancelCaravan.hotKey = KeyBindingDefOf.Designator_Cancel;
						this.$current = cancelCaravan;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					if (pawn.Spawned)
					{
						anyCaravanToJoin = false;
						for (int i = 0; i < pawn.Map.lordManager.lords.Count; i++)
						{
							Lord lord2 = pawn.Map.lordManager.lords[i];
							if (lord2.faction == Faction.OfPlayer && lord2.LordJob is LordJob_FormAndSendCaravan)
							{
								anyCaravanToJoin = true;
								break;
							}
						}
						if (anyCaravanToJoin && Dialog_FormCaravan.AllSendablePawns(pawn.Map, false).Contains(pawn))
						{
							Command_Action addToCaravan = new Command_Action();
							addToCaravan.defaultLabel = "CommandAddToCaravan".Translate();
							addToCaravan.defaultDesc = "CommandAddToCaravanDesc".Translate();
							addToCaravan.icon = CaravanFormingUtility.AddToCaravanCommand;
							addToCaravan.action = delegate()
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
							};
							addToCaravan.hotKey = KeyBindingDefOf.Misc7;
							this.$current = addToCaravan;
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							return true;
						}
					}
					break;
				case 1u:
				{
					Command_Action removeFromCaravan = new Command_Action();
					removeFromCaravan.defaultLabel = "CommandRemoveFromCaravan".Translate();
					removeFromCaravan.defaultDesc = "CommandRemoveFromCaravanDesc".Translate();
					removeFromCaravan.icon = CaravanFormingUtility.RemoveFromCaravanCommand;
					removeFromCaravan.action = delegate()
					{
						CaravanFormingUtility.RemovePawnFromCaravan(<GetGizmos>c__AnonStorey2.<>f__ref$2.pawn, <GetGizmos>c__AnonStorey2.lord);
					};
					removeFromCaravan.hotKey = KeyBindingDefOf.Misc6;
					this.$current = removeFromCaravan;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				case 2u:
					break;
				case 3u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CaravanFormingUtility.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new CaravanFormingUtility.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.pawn = pawn;
				return <GetGizmos>c__Iterator;
			}

			private sealed class <GetGizmos>c__AnonStorey2
			{
				internal Pawn pawn;

				public <GetGizmos>c__AnonStorey2()
				{
				}

				internal void <>m__0()
				{
					List<Lord> list = new List<Lord>();
					for (int i = 0; i < this.pawn.Map.lordManager.lords.Count; i++)
					{
						Lord lord = this.pawn.Map.lordManager.lords[i];
						if (lord.faction == Faction.OfPlayer && lord.LordJob is LordJob_FormAndSendCaravan)
						{
							list.Add(lord);
						}
					}
					if (list.Count != 0)
					{
						if (list.Count == 1)
						{
							CaravanFormingUtility.LateJoinFormingCaravan(this.pawn, list[0]);
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
						else
						{
							List<FloatMenuOption> list2 = new List<FloatMenuOption>();
							for (int j = 0; j < list.Count; j++)
							{
								Lord caravanLocal = list[j];
								string label = "Caravan".Translate() + " " + (j + 1);
								list2.Add(new FloatMenuOption(label, delegate()
								{
									if (this.pawn.Spawned && this.pawn.Map.lordManager.lords.Contains(caravanLocal) && Dialog_FormCaravan.AllSendablePawns(this.pawn.Map, false).Contains(this.pawn))
									{
										CaravanFormingUtility.LateJoinFormingCaravan(this.pawn, caravanLocal);
									}
								}, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
							Find.WindowStack.Add(new FloatMenu(list2));
						}
					}
				}

				private sealed class <GetGizmos>c__AnonStorey3
				{
					internal Lord caravanLocal;

					internal CaravanFormingUtility.<GetGizmos>c__Iterator0.<GetGizmos>c__AnonStorey2 <>f__ref$2;

					public <GetGizmos>c__AnonStorey3()
					{
					}

					internal void <>m__0()
					{
						if (this.<>f__ref$2.pawn.Spawned && this.<>f__ref$2.pawn.Map.lordManager.lords.Contains(this.caravanLocal) && Dialog_FormCaravan.AllSendablePawns(this.<>f__ref$2.pawn.Map, false).Contains(this.<>f__ref$2.pawn))
						{
							CaravanFormingUtility.LateJoinFormingCaravan(this.<>f__ref$2.pawn, this.caravanLocal);
						}
					}
				}
			}

			private sealed class <GetGizmos>c__AnonStorey1
			{
				internal Lord lord;

				internal CaravanFormingUtility.<GetGizmos>c__Iterator0 <>f__ref$0;

				internal CaravanFormingUtility.<GetGizmos>c__Iterator0.<GetGizmos>c__AnonStorey2 <>f__ref$2;

				public <GetGizmos>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					CaravanFormingUtility.StopFormingCaravan(this.lord);
				}

				internal void <>m__1()
				{
					CaravanFormingUtility.RemovePawnFromCaravan(this.<>f__ref$2.pawn, this.lord);
				}
			}
		}
	}
}
