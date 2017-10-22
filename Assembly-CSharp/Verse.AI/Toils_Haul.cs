using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	public class Toils_Haul
	{
		public static void ErrorCheckForCarry(Pawn pawn, Thing haulThing)
		{
			if (!haulThing.Spawned)
			{
				Log.Message(pawn + " tried to start carry " + haulThing + " which isn't spawned.");
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
			}
			if (haulThing.stackCount == 0)
			{
				Log.Message(pawn + " tried to start carry " + haulThing + " which had stackcount 0.");
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
			}
			if (pawn.jobs.curJob.count <= 0)
			{
				Log.Error("Invalid count: " + pawn.jobs.curJob.count + ", setting to 1. Job was " + pawn.jobs.curJob);
				pawn.jobs.curJob.count = 1;
			}
		}

		public static Toil StartCarryThing(TargetIndex haulableInd, bool putRemainderInQueue = false, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = false)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(haulableInd).Thing;
				Toils_Haul.ErrorCheckForCarry(actor, thing);
				if (curJob.count == 0)
				{
					throw new Exception("StartCarryThing job had count = " + curJob.count + ". Job: " + curJob);
				}
				int num = actor.carryTracker.AvailableStackSpace(thing.def);
				if (num == 0)
				{
					throw new Exception("StartCarryThing got availableStackSpace " + num + " for haulTarg " + thing + ". Job: " + curJob);
				}
				if (failIfStackCountLessThanJobCount && thing.stackCount < curJob.count)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
				else
				{
					int num2 = Mathf.Min(curJob.count, num, thing.stackCount);
					if (num2 <= 0)
					{
						throw new Exception("StartCarryThing desiredNumToTake = " + num2);
					}
					int stackCount = thing.stackCount;
					int num3 = actor.carryTracker.TryStartCarry(thing, num2, true);
					if (num3 == 0)
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					if (num3 < stackCount)
					{
						int num4 = curJob.count - num3;
						if (putRemainderInQueue && num4 > 0)
						{
							curJob.GetTargetQueue(haulableInd).Insert(0, thing);
							if (curJob.countQueue == null)
							{
								curJob.countQueue = new List<int>();
							}
							curJob.countQueue.Insert(0, num4);
						}
						else if (actor.Map.reservationManager.ReservedBy(thing, actor, curJob))
						{
							actor.Map.reservationManager.Release(thing, actor, curJob);
						}
					}
					if (subtractNumTakenFromJobCount)
					{
						curJob.count -= num3;
					}
					curJob.SetTarget(haulableInd, actor.carryTracker.CarriedThing);
					actor.records.Increment(RecordDefOf.ThingsHauled);
				}
			};
			return toil;
		}

		public static Toil JumpIfAlsoCollectingNextTargetInQueue(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (!targetQueue.NullOrEmpty() && curJob.count > 0)
				{
					if (actor.carryTracker.CarriedThing == null)
					{
						Log.Error("JumpToAlsoCollectTargetInQueue run on " + actor + " who is not carrying something.");
					}
					else if (actor.carryTracker.AvailableStackSpace(actor.carryTracker.CarriedThing.def) > 0)
					{
						int num = 0;
						while (true)
						{
							if (num < targetQueue.Count)
							{
								if (GenAI.CanUseItemForWork(actor, targetQueue[num].Thing))
								{
									if (targetQueue[num].Thing.def != actor.carryTracker.CarriedThing.def)
									{
										num++;
										continue;
									}
									break;
								}
								actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
							}
							return;
						}
						curJob.SetTarget(ind, targetQueue[num].Thing);
						targetQueue.RemoveAt(num);
						actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
					}
				}
			};
			return toil;
		}

		public static Toil CheckForGetOpportunityDuplicate(Toil getHaulTargetToil, TargetIndex haulableInd, TargetIndex storeCellInd, bool takeFromValidStorage = false, Predicate<Thing> extraValidator = null)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing.def.stackLimit != 1 && !actor.carryTracker.Full && curJob.count > 0)
				{
					Thing thing = null;
					Predicate<Thing> validator = (Predicate<Thing>)((Thing t) => (byte)(t.Spawned ? ((t.def == actor.carryTracker.CarriedThing.def) ? (t.CanStackWith(actor.carryTracker.CarriedThing) ? ((!t.IsForbidden(actor)) ? ((takeFromValidStorage || !t.IsInValidStorage()) ? ((storeCellInd == TargetIndex.None || curJob.GetTarget(storeCellInd).Cell.IsValidStorageFor(actor.Map, t)) ? (actor.CanReserve(t, 1, -1, null, false) ? (((object)extraValidator == null || extraValidator(t)) ? 1 : 0) : 0) : 0) : 0) : 0) : 0) : 0) : 0) != 0);
					thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.ClosestTouch, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), 8f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
					if (thing != null)
					{
						curJob.SetTarget(haulableInd, thing);
						actor.jobs.curDriver.JumpToToil(getHaulTargetToil);
					}
				}
			};
			return toil;
		}

		public static Toil CarryHauledThingToCell(TargetIndex squareIndex)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				IntVec3 cell2 = toil.actor.jobs.curJob.GetTarget(squareIndex).Cell;
				toil.actor.pather.StartPath(cell2, PathEndMode.ClosestTouch);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.AddFailCondition((Func<bool>)delegate()
			{
				Pawn actor = toil.actor;
				IntVec3 cell = actor.jobs.curJob.GetTarget(squareIndex).Cell;
				return (byte)((actor.jobs.curJob.haulMode == HaulMode.ToCellStorage && !cell.IsValidStorageFor(actor.Map, actor.carryTracker.CarriedThing)) ? 1 : 0) != 0;
			});
			return toil;
		}

		public static Toil PlaceCarriedThingInCellFacing(TargetIndex facingTargetInd)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error(actor + " tried to place hauled thing in facing cell but is not hauling anything.");
				}
				else
				{
					LocalTargetInfo target = actor.CurJob.GetTarget(facingTargetInd);
					IntVec3 b = (!target.HasThing) ? target.Cell : target.Thing.OccupiedRect().ClosestCellTo(actor.Position);
					IntVec3 dropLoc = actor.Position + Pawn_RotationTracker.RotFromAngleBiased((actor.Position - b).AngleFlat).FacingCell;
					Thing thing = default(Thing);
					if (!actor.carryTracker.TryDropCarriedThing(dropLoc, ThingPlaceMode.Direct, out thing, (Action<Thing, int>)null))
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
				}
			};
			return toil;
		}

		public static Toil PlaceHauledThingInCell(TargetIndex cellInd, Toil nextToilOnPlaceFailOrIncomplete, bool storageMode)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				IntVec3 cell = curJob.GetTarget(cellInd).Cell;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error(actor + " tried to place hauled thing in cell but is not hauling anything.");
				}
				else
				{
					SlotGroup slotGroup = actor.Map.slotGroupManager.SlotGroupAt(cell);
					if (slotGroup != null && slotGroup.Settings.AllowedToAccept(actor.carryTracker.CarriedThing))
					{
						actor.Map.designationManager.RemoveAllDesignationsOn(actor.carryTracker.CarriedThing, false);
					}
					Action<Thing, int> placedAction = null;
					if (curJob.def == JobDefOf.DoBill)
					{
						placedAction = (Action<Thing, int>)delegate(Thing th, int added)
						{
							if (curJob.placedThings == null)
							{
								curJob.placedThings = new List<ThingStackPartClass>();
							}
							ThingStackPartClass thingStackPartClass = curJob.placedThings.Find((Predicate<ThingStackPartClass>)((ThingStackPartClass x) => x.thing == th));
							if (thingStackPartClass != null)
							{
								thingStackPartClass.Count += added;
							}
							else
							{
								curJob.placedThings.Add(new ThingStackPartClass(th, added));
							}
						};
					}
					Thing thing = default(Thing);
					if (!actor.carryTracker.TryDropCarriedThing(cell, ThingPlaceMode.Direct, out thing, placedAction))
					{
						if (storageMode)
						{
							IntVec3 c = default(IntVec3);
							if (nextToilOnPlaceFailOrIncomplete != null && StoreUtility.TryFindBestBetterStoreCellFor(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, out c, true))
							{
								if (actor.CanReserve(c, 1, -1, null, false))
								{
									actor.Reserve(c, actor.CurJob, 1, -1, null);
								}
								actor.CurJob.SetTarget(cellInd, c);
								actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
							}
							else
							{
								Job job = HaulAIUtility.HaulAsideJobFor(actor, actor.carryTracker.CarriedThing);
								if (job != null)
								{
									curJob.targetA = job.targetA;
									curJob.targetB = job.targetB;
									curJob.targetC = job.targetC;
									curJob.count = job.count;
									curJob.haulOpportunisticDuplicates = job.haulOpportunisticDuplicates;
									curJob.haulMode = job.haulMode;
									actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
								}
								else
								{
									Log.Error("Incomplete haul for " + actor + ": Could not find anywhere to put " + actor.carryTracker.CarriedThing + " near " + actor.Position + ". Destroying. This should never happen!");
									actor.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
								}
							}
						}
						else if (nextToilOnPlaceFailOrIncomplete != null)
						{
							actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
						}
					}
				}
			};
			return toil;
		}

		public static Toil CarryHauledThingToContainer()
		{
			Toil gotoDest = new Toil();
			gotoDest.initAction = (Action)delegate
			{
				gotoDest.actor.pather.StartPath(gotoDest.actor.jobs.curJob.targetB.Thing, PathEndMode.Touch);
			};
			gotoDest.AddFailCondition((Func<bool>)delegate
			{
				Thing thing = gotoDest.actor.jobs.curJob.targetB.Thing;
				bool result;
				if (thing.Destroyed || thing.IsForbidden(gotoDest.actor))
				{
					result = true;
				}
				else
				{
					ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
					result = ((byte)((thingOwner != null && !thingOwner.CanAcceptAnyOf(gotoDest.actor.carryTracker.CarriedThing, true)) ? 1 : 0) != 0);
				}
				return result;
			});
			gotoDest.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return gotoDest;
		}

		public static Toil DepositHauledThingInContainer(TargetIndex containerInd, TargetIndex reserveForContainerInd)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error(actor + " tried to place hauled thing in container but is not hauling anything.");
				}
				else
				{
					Thing thing = curJob.GetTarget(containerInd).Thing;
					ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
					if (thingOwner != null)
					{
						int num = actor.carryTracker.CarriedThing.stackCount;
						if (thing is IConstructible)
						{
							int a = GenConstruct.AmountNeededByOf((IConstructible)thing, actor.carryTracker.CarriedThing.def);
							num = Mathf.Min(a, num);
							if (reserveForContainerInd != 0)
							{
								Thing thing2 = curJob.GetTarget(reserveForContainerInd).Thing;
								if (thing2 != null && thing2 != thing)
								{
									int num2 = GenConstruct.AmountNeededByOf((IConstructible)thing2, actor.carryTracker.CarriedThing.def);
									num = Mathf.Min(num, actor.carryTracker.CarriedThing.stackCount - num2);
								}
							}
						}
						actor.carryTracker.innerContainer.TryTransferToContainer(actor.carryTracker.CarriedThing, thingOwner, num, true);
					}
					else if (curJob.GetTarget(containerInd).Thing.def.Minifiable)
					{
						actor.carryTracker.innerContainer.Clear();
					}
					else
					{
						Log.Error("Could not deposit hauled thing in container: " + curJob.GetTarget(containerInd).Thing);
					}
				}
			};
			return toil;
		}

		public static Toil JumpToCarryToNextContainerIfPossible(Toil carryToContainerToil, TargetIndex primaryTargetInd)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing != null && curJob.targetQueueB != null && curJob.targetQueueB.Count > 0)
				{
					Thing primaryTarget = curJob.GetTarget(primaryTargetInd).Thing;
					bool hasSpareItems = actor.carryTracker.CarriedThing.stackCount > GenConstruct.AmountNeededByOf((IConstructible)primaryTarget, actor.carryTracker.CarriedThing.def);
					Predicate<Thing> validator = (Predicate<Thing>)((Thing th) => (byte)(GenConstruct.CanConstruct(th, actor, false) ? (((IConstructible)th).MaterialsNeeded().Any((Predicate<ThingCountClass>)((ThingCountClass need) => need.thingDef == actor.carryTracker.CarriedThing.def)) ? ((th == primaryTarget || hasSpareItems) ? 1 : 0) : 0) : 0) != 0);
					Thing nextTarget = GenClosest.ClosestThing_Global_Reachable(actor.Position, actor.Map, from targ in curJob.targetQueueB
					select targ.Thing, PathEndMode.Touch, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false), 99999f, validator, null);
					if (nextTarget != null)
					{
						curJob.targetQueueB.RemoveAll((Predicate<LocalTargetInfo>)((LocalTargetInfo targ) => targ.Thing == nextTarget));
						curJob.targetB = nextTarget;
						actor.jobs.curDriver.JumpToToil(carryToContainerToil);
					}
				}
			};
			return toil;
		}

		public static Toil TakeToInventory(TargetIndex ind, int count)
		{
			return Toils_Haul.TakeToInventory(ind, (Func<int>)(() => count));
		}

		public static Toil TakeToInventory(TargetIndex ind, Func<int> countGetter)
		{
			Toil takeThing = new Toil();
			takeThing.initAction = (Action)delegate()
			{
				Pawn actor = takeThing.actor;
				Thing thing = actor.CurJob.GetTarget(ind).Thing;
				Toils_Haul.ErrorCheckForCarry(actor, thing);
				int num = Mathf.Min(countGetter(), thing.stackCount);
				if (num <= 0)
				{
					actor.jobs.curDriver.ReadyForNextToil();
				}
				else
				{
					actor.inventory.GetDirectlyHeldThings().TryAdd(thing.SplitOff(num), true);
					if (thing.def.ingestible != null && (int)thing.def.ingestible.preferability <= 5)
					{
						actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
					}
					thing.def.soundPickup.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
				}
			};
			return takeThing;
		}
	}
}
