using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobDriver_TakeToBed : JobDriver
	{
		private const TargetIndex TakeeIndex = TargetIndex.A;

		private const TargetIndex BedIndex = TargetIndex.B;

		public JobDriver_TakeToBed()
		{
		}

		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Building_Bed DropBed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.Takee;
			Job job = this.job;
			bool result;
			if (pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
			{
				pawn = this.pawn;
				target = this.DropBed;
				job = this.job;
				int sleepingSlotsCount = this.DropBed.SleepingSlotsCount;
				int stackCount = 0;
				result = pawn.Reserve(target, job, sleepingSlotsCount, stackCount, null, errorOnFailed);
			}
			else
			{
				result = false;
			}
			return result;
		}

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

		private void CheckMakeTakeeGuest()
		{
			if (!this.job.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan())
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, false);
			}
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <checkArrestResistance>__0;

			internal Toil <startCarrying>__0;

			internal Toil <makePrisonerAndInit>__0;

			internal Toil <tuckIntoBed>__0;

			internal JobDriver_TakeToBed $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.FailOnDestroyedOrNull(TargetIndex.A);
					this.FailOnDestroyedOrNull(TargetIndex.B);
					this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
					this.FailOn(delegate()
					{
						if (this.job.def.makeTargetPrisoner)
						{
							if (!base.DropBed.ForPrisoners)
							{
								return true;
							}
						}
						else if (base.DropBed.ForPrisoners != base.Takee.IsPrisoner)
						{
							return true;
						}
						return false;
					});
					this.$current = Toils_Bed.ClaimBedIfNonMedical(TargetIndex.B, TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					base.AddFinishAction(delegate
					{
						if (this.job.def.makeTargetPrisoner && base.Takee.ownership.OwnedBed == base.DropBed && base.Takee.Position != RestUtility.GetBedSleepingSlotPosFor(base.Takee, base.DropBed))
						{
							base.Takee.ownership.UnclaimBed();
						}
					});
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.job.def == JobDefOf.Arrest && !base.Takee.CanBeArrestedBy(this.pawn)).FailOn(() => !this.pawn.CanReach(base.DropBed, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)).FailOn(() => this.job.def == JobDefOf.Rescue && !base.Takee.Downed).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
				{
					Toil checkArrestResistance = new Toil();
					checkArrestResistance.initAction = delegate()
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
					};
					this.$current = checkArrestResistance;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				case 3u:
					startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false).FailOnNonMedicalBedNotOwned(TargetIndex.B, TargetIndex.A);
					startCarrying.AddPreInitAction(new Action(base.CheckMakeTakeeGuest));
					this.$current = startCarrying;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
				{
					Toil makePrisonerAndInit = new Toil();
					makePrisonerAndInit.initAction = delegate()
					{
						base.CheckMakeTakeePrisoner();
						if (base.Takee.playerSettings == null)
						{
							base.Takee.playerSettings = new Pawn_PlayerSettings(base.Takee);
						}
					};
					this.$current = makePrisonerAndInit;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				case 6u:
					this.$current = Toils_Reserve.Release(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
				{
					Toil tuckIntoBed = new Toil();
					tuckIntoBed.initAction = delegate()
					{
						IntVec3 position = base.DropBed.Position;
						Thing thing;
						this.pawn.carryTracker.TryDropCarriedThing(position, ThingPlaceMode.Direct, out thing, null);
						if (!base.DropBed.Destroyed && (base.DropBed.owners.Contains(base.Takee) || (base.DropBed.Medical && base.DropBed.AnyUnoccupiedSleepingSlot) || base.Takee.ownership == null))
						{
							base.Takee.jobs.Notify_TuckedIntoBed(base.DropBed);
							if (base.Takee.RaceProps.Humanlike && this.job.def != JobDefOf.Arrest && !base.Takee.IsPrisonerOfColony)
							{
								base.Takee.relations.Notify_RescuedBy(this.pawn);
							}
							base.Takee.mindState.Notify_TuckedIntoBed();
						}
						if (base.Takee.IsPrisonerOfColony)
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.PrisonerTab, base.Takee, OpportunityType.GoodToKnow);
						}
					};
					tuckIntoBed.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = tuckIntoBed;
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				}
				case 8u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Toil IEnumerator<Toil>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_TakeToBed.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_TakeToBed.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				if (this.job.def.makeTargetPrisoner)
				{
					if (!base.DropBed.ForPrisoners)
					{
						return true;
					}
				}
				else if (base.DropBed.ForPrisoners != base.Takee.IsPrisoner)
				{
					return true;
				}
				return false;
			}

			internal void <>m__1()
			{
				if (this.job.def.makeTargetPrisoner && base.Takee.ownership.OwnedBed == base.DropBed && base.Takee.Position != RestUtility.GetBedSleepingSlotPosFor(base.Takee, base.DropBed))
				{
					base.Takee.ownership.UnclaimBed();
				}
			}

			internal bool <>m__2()
			{
				return this.job.def == JobDefOf.Arrest && !base.Takee.CanBeArrestedBy(this.pawn);
			}

			internal bool <>m__3()
			{
				return !this.pawn.CanReach(base.DropBed, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
			}

			internal bool <>m__4()
			{
				return this.job.def == JobDefOf.Rescue && !base.Takee.Downed;
			}

			internal void <>m__5()
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

			internal void <>m__6()
			{
				base.CheckMakeTakeePrisoner();
				if (base.Takee.playerSettings == null)
				{
					base.Takee.playerSettings = new Pawn_PlayerSettings(base.Takee);
				}
			}

			internal void <>m__7()
			{
				IntVec3 position = base.DropBed.Position;
				Thing thing;
				this.pawn.carryTracker.TryDropCarriedThing(position, ThingPlaceMode.Direct, out thing, null);
				if (!base.DropBed.Destroyed && (base.DropBed.owners.Contains(base.Takee) || (base.DropBed.Medical && base.DropBed.AnyUnoccupiedSleepingSlot) || base.Takee.ownership == null))
				{
					base.Takee.jobs.Notify_TuckedIntoBed(base.DropBed);
					if (base.Takee.RaceProps.Humanlike && this.job.def != JobDefOf.Arrest && !base.Takee.IsPrisonerOfColony)
					{
						base.Takee.relations.Notify_RescuedBy(this.pawn);
					}
					base.Takee.mindState.Notify_TuckedIntoBed();
				}
				if (base.Takee.IsPrisonerOfColony)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.PrisonerTab, base.Takee, OpportunityType.GoodToKnow);
				}
			}
		}
	}
}
