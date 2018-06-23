using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000082 RID: 130
	public class JobDriver_TakeToBed : JobDriver
	{
		// Token: 0x0400023E RID: 574
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x0400023F RID: 575
		private const TargetIndex BedIndex = TargetIndex.B;

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00025C74 File Offset: 0x00024074
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00025CA4 File Offset: 0x000240A4
		protected Building_Bed DropBed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00025CD4 File Offset: 0x000240D4
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null) && this.pawn.Reserve(this.DropBed, this.job, this.DropBed.SleepingSlotsCount, 0, null);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00025D3C File Offset: 0x0002413C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			this.FailOn(delegate()
			{
				if (this.job.def.makeTargetPrisoner)
				{
					if (!this.DropBed.ForPrisoners)
					{
						return true;
					}
				}
				else if (this.DropBed.ForPrisoners != this.Takee.IsPrisoner)
				{
					return true;
				}
				return false;
			});
			yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.B, TargetIndex.A);
			base.AddFinishAction(delegate
			{
				if (this.job.def.makeTargetPrisoner && this.Takee.ownership.OwnedBed == this.DropBed && this.Takee.Position != RestUtility.GetBedSleepingSlotPosFor(this.Takee, this.DropBed))
				{
					this.Takee.ownership.UnclaimBed();
				}
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.job.def == JobDefOf.Arrest && !this.Takee.CanBeArrestedBy(this.pawn)).FailOn(() => !this.pawn.CanReach(this.DropBed, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)).FailOn(() => this.job.def == JobDefOf.Rescue && !this.Takee.Downed).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.job.def.makeTargetPrisoner)
					{
						Pawn pawn = (Pawn)this.job.targetA.Thing;
						Lord lord = pawn.GetLord();
						if (lord != null)
						{
							lord.Notify_PawnAttemptArrested(pawn);
						}
						GenClamor.DoClamor(pawn, 10f, ClamorDefOf.Harm);
						if (this.job.def == JobDefOf.Arrest && !pawn.CheckAcceptArrest(this.pawn))
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
						}
					}
				}
			};
			Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false).FailOnNonMedicalBedNotOwned(TargetIndex.B, TargetIndex.A);
			startCarrying.AddPreInitAction(new Action(this.CheckMakeTakeeGuest));
			yield return startCarrying;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.CheckMakeTakeePrisoner();
					if (this.Takee.playerSettings == null)
					{
						this.Takee.playerSettings = new Pawn_PlayerSettings(this.Takee);
					}
				}
			};
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield return new Toil
			{
				initAction = delegate()
				{
					IntVec3 position = this.DropBed.Position;
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(position, ThingPlaceMode.Direct, out thing, null);
					if (!this.DropBed.Destroyed && (this.DropBed.owners.Contains(this.Takee) || (this.DropBed.Medical && this.DropBed.AnyUnoccupiedSleepingSlot) || this.Takee.ownership == null))
					{
						this.Takee.jobs.Notify_TuckedIntoBed(this.DropBed);
						if (this.Takee.RaceProps.Humanlike && this.job.def != JobDefOf.Arrest && !this.Takee.IsPrisonerOfColony)
						{
							this.Takee.relations.Notify_RescuedBy(this.pawn);
						}
						this.Takee.mindState.Notify_TuckedIntoBed();
					}
					if (this.Takee.IsPrisonerOfColony)
					{
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.PrisonerTab, this.Takee, OpportunityType.GoodToKnow);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00025D68 File Offset: 0x00024168
		private void CheckMakeTakeePrisoner()
		{
			if (this.job.def.makeTargetPrisoner)
			{
				if (this.Takee.guest.Released)
				{
					this.Takee.guest.Released = false;
					this.Takee.guest.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
				}
				if (!this.Takee.IsPrisonerOfColony)
				{
					this.Takee.guest.CapturedBy(Faction.OfPlayer, this.pawn);
				}
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00025DF8 File Offset: 0x000241F8
		private void CheckMakeTakeeGuest()
		{
			if (!this.job.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan())
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, false);
			}
		}
	}
}
