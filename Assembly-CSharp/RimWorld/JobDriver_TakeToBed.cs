using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobDriver_TakeToBed : JobDriver
	{
		private const TargetIndex TakeeIndex = TargetIndex.A;

		private const TargetIndex BedIndex = TargetIndex.B;

		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Building_Bed DropBed
		{
			get
			{
				return (Building_Bed)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn((Func<bool>)delegate
			{
				if (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.CurJob.def.makeTargetPrisoner)
				{
					if (!((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.DropBed.ForPrisoners)
					{
						return true;
					}
				}
				else if (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.DropBed.ForPrisoners != ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.Takee.IsPrisoner)
				{
					return true;
				}
				return false;
			});
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.B, this.DropBed.SleepingSlotsCount, 0, null);
			yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.B, TargetIndex.A);
			base.AddFinishAction((Action)delegate
			{
				if (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_00e8: stateMachine*/)._003C_003Ef__this.CurJob.def.makeTargetPrisoner && ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_00e8: stateMachine*/)._003C_003Ef__this.Takee.ownership.OwnedBed == ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_00e8: stateMachine*/)._003C_003Ef__this.DropBed && ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_00e8: stateMachine*/)._003C_003Ef__this.Takee.Position != RestUtility.GetBedSleepingSlotPosFor(((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_00e8: stateMachine*/)._003C_003Ef__this.Takee, ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_00e8: stateMachine*/)._003C_003Ef__this.DropBed))
				{
					((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_00e8: stateMachine*/)._003C_003Ef__this.Takee.ownership.UnclaimBed();
				}
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnNonMedicalBedNotOwned(TargetIndex.B, TargetIndex.A).FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0114: stateMachine*/)._003C_003Ef__this.CurJob.def == JobDefOf.Arrest && !((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0114: stateMachine*/)._003C_003Ef__this.Takee.CanBeArrested())).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.pawn.CanReach((Thing)((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.DropBed, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))).FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0136: stateMachine*/)._003C_003Ef__this.CurJob.def == JobDefOf.Rescue && !((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0136: stateMachine*/)._003C_003Ef__this.Takee.Downed)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_016f: stateMachine*/)._003C_003Ef__this.CurJob.def.makeTargetPrisoner)
					{
						Pawn pawn = (Pawn)((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_016f: stateMachine*/)._003C_003Ef__this.CurJob.targetA.Thing;
						Lord lord = pawn.GetLord();
						if (lord != null)
						{
							lord.Notify_PawnAttemptArrested(pawn);
						}
						GenClamor.DoClamor(pawn, 10f, ClamorType.Harm);
						if (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_016f: stateMachine*/)._003C_003Ef__this.CurJob.def == JobDefOf.Arrest && !pawn.CheckAcceptArrest(((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_016f: stateMachine*/)._003C_003Ef__this.pawn))
						{
							((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_016f: stateMachine*/)._003C_003Ef__this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
						}
					}
				}
			};
			Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			startCarrying.AddPreInitAction(new Action(this.CheckMakeTakeeGuest));
			yield return startCarrying;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0204: stateMachine*/)._003C_003Ef__this.CheckMakeTakeePrisoner();
					if (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0204: stateMachine*/)._003C_003Ef__this.Takee.playerSettings == null)
					{
						((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0204: stateMachine*/)._003C_003Ef__this.Takee.playerSettings = new Pawn_PlayerSettings(((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0204: stateMachine*/)._003C_003Ef__this.Takee);
					}
				}
			};
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					IntVec3 position = ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.DropBed.Position;
					Thing thing = default(Thing);
					((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.pawn.carryTracker.TryDropCarriedThing(position, ThingPlaceMode.Direct, out thing, (Action<Thing, int>)null);
					if (!((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.DropBed.Destroyed && (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.DropBed.owners.Contains(((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.Takee) || (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.DropBed.Medical && ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.DropBed.AnyUnoccupiedSleepingSlot) || ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.Takee.ownership == null))
					{
						((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.Takee.jobs.Notify_TuckedIntoBed(((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.DropBed);
						if (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.Takee.RaceProps.Humanlike && ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.CurJob.def != JobDefOf.Arrest && !((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.Takee.IsPrisonerOfColony)
						{
							((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.Takee.relations.Notify_RescuedBy(((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.pawn);
						}
					}
					if (((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.Takee.IsPrisonerOfColony)
					{
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.PrisonerTab, ((_003CMakeNewToils_003Ec__Iterator3E)/*Error near IL_0257: stateMachine*/)._003C_003Ef__this.Takee, OpportunityType.GoodToKnow);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		private void CheckMakeTakeePrisoner()
		{
			if (base.CurJob.def.makeTargetPrisoner)
			{
				if (this.Takee.guest.released)
				{
					this.Takee.guest.released = false;
					this.Takee.guest.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
				}
				if (!this.Takee.IsPrisonerOfColony)
				{
					if (this.Takee.Faction != null)
					{
						this.Takee.Faction.Notify_MemberCaptured(this.Takee, base.pawn.Faction);
					}
					this.Takee.guest.SetGuestStatus(Faction.OfPlayer, true);
					if (this.Takee.guest.IsPrisoner)
					{
						TaleRecorder.RecordTale(TaleDefOf.Captured, base.pawn, this.Takee);
						base.pawn.records.Increment(RecordDefOf.PeopleCaptured);
					}
				}
			}
		}

		private void CheckMakeTakeeGuest()
		{
			if (!base.CurJob.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null)
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, false);
			}
		}
	}
}
