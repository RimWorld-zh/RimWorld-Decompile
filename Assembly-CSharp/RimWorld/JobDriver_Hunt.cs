using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Hunt : JobDriver
	{
		private int jobStartTick = -1;

		private const TargetIndex VictimInd = TargetIndex.A;

		private const TargetIndex CorpseInd = TargetIndex.A;

		private const TargetIndex StoreCellInd = TargetIndex.B;

		private const int MaxHuntTicks = 5000;

		public JobDriver_Hunt()
		{
		}

		public Pawn Victim
		{
			get
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jobStartTick, "jobStartTick", 0, false);
		}

		public override string GetReport()
		{
			if (this.Victim != null)
			{
				return this.job.def.reportString.Replace("TargetA", this.Victim.LabelShort);
			}
			return base.GetReport();
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.Victim;
			Job job = this.job;
			return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(delegate()
			{
				if (!this.job.ignoreDesignations)
				{
					Pawn victim = this.Victim;
					if (victim != null && !victim.Dead && base.Map.designationManager.DesignationOn(victim, DesignationDefOf.Hunt) == null)
					{
						return true;
					}
				}
				return false;
			});
			yield return new Toil
			{
				initAction = delegate()
				{
					this.jobStartTick = Find.TickManager.TicksGame;
				}
			};
			yield return Toils_Combat.TrySetJobToUseAttackVerb(TargetIndex.A);
			Toil startCollectCorpseLabel = Toils_General.Label();
			Toil slaughterLabel = Toils_General.Label();
			Toil gotoCastPos = Toils_Combat.GotoCastPosition(TargetIndex.A, true, 0.95f).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel).FailOn(() => Find.TickManager.TicksGame > this.jobStartTick + 5000);
			yield return gotoCastPos;
			Toil slaughterIfPossible = Toils_Jump.JumpIf(slaughterLabel, delegate
			{
				Pawn victim = this.Victim;
				return (victim.RaceProps.DeathActionWorker == null || !victim.RaceProps.DeathActionWorker.DangerousInMelee) && victim.Downed;
			});
			yield return slaughterIfPossible;
			yield return Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, gotoCastPos);
			yield return Toils_Combat.CastVerb(TargetIndex.A, false).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel).FailOn(() => Find.TickManager.TicksGame > this.jobStartTick + 5000);
			yield return Toils_Jump.JumpIfTargetDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel);
			yield return Toils_Jump.Jump(slaughterIfPossible);
			yield return slaughterLabel;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnMobile(TargetIndex.A);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false).FailOnMobile(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				if (this.Victim.Dead)
				{
					return;
				}
				ExecutionUtility.DoExecutionByCut(this.pawn, this.Victim);
				this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				if (this.pawn.InMentalState)
				{
					this.pawn.MentalState.Notify_SlaughteredAnimal();
				}
			});
			yield return Toils_Jump.Jump(startCollectCorpseLabel);
			yield return startCollectCorpseLabel;
			yield return this.StartCollectCorpseToil();
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
			yield break;
		}

		private Toil StartCollectCorpseToil()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (this.Victim == null)
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.Hunted, new object[]
				{
					this.pawn,
					this.Victim
				});
				Corpse corpse = this.Victim.Corpse;
				if (corpse == null || !this.pawn.CanReserveAndReach(corpse, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					return;
				}
				corpse.SetForbidden(false, true);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(corpse, this.pawn, this.Map, StoragePriority.Unstored, this.pawn.Faction, out c, true))
				{
					this.pawn.Reserve(corpse, this.job, 1, -1, null, true);
					this.pawn.Reserve(c, this.job, 1, -1, null, true);
					this.job.SetTarget(TargetIndex.B, c);
					this.job.SetTarget(TargetIndex.A, corpse);
					this.job.count = 1;
					this.job.haulMode = HaulMode.ToCellStorage;
					return;
				}
				this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
			};
			return toil;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <init>__0;

			internal Toil <startCollectCorpseLabel>__0;

			internal Toil <slaughterLabel>__0;

			internal Toil <gotoCastPos>__0;

			internal Toil <slaughterIfPossible>__0;

			internal Toil <carryToCell>__0;

			internal JobDriver_Hunt $this;

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
				{
					this.FailOn(delegate()
					{
						if (!this.job.ignoreDesignations)
						{
							Pawn victim = base.Victim;
							if (victim != null && !victim.Dead && base.Map.designationManager.DesignationOn(victim, DesignationDefOf.Hunt) == null)
							{
								return true;
							}
						}
						return false;
					});
					Toil init = new Toil();
					init.initAction = delegate()
					{
						this.jobStartTick = Find.TickManager.TicksGame;
					};
					this.$current = init;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
					this.$current = Toils_Combat.TrySetJobToUseAttackVerb(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					startCollectCorpseLabel = Toils_General.Label();
					slaughterLabel = Toils_General.Label();
					gotoCastPos = Toils_Combat.GotoCastPosition(TargetIndex.A, true, 0.95f).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel).FailOn(() => Find.TickManager.TicksGame > this.jobStartTick + 5000);
					this.$current = gotoCastPos;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					slaughterIfPossible = Toils_Jump.JumpIf(slaughterLabel, delegate
					{
						Pawn victim = base.Victim;
						return (victim.RaceProps.DeathActionWorker == null || !victim.RaceProps.DeathActionWorker.DangerousInMelee) && victim.Downed;
					});
					this.$current = slaughterIfPossible;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, gotoCastPos);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Combat.CastVerb(TargetIndex.A, false).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel).FailOn(() => Find.TickManager.TicksGame > this.jobStartTick + 5000);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Jump.JumpIfTargetDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = Toils_Jump.Jump(slaughterIfPossible);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = slaughterLabel;
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnMobile(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				case 10u:
					this.$current = Toils_General.WaitWith(TargetIndex.A, 180, true, false).FailOnMobile(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				case 11u:
					this.$current = Toils_General.Do(delegate
					{
						if (base.Victim.Dead)
						{
							return;
						}
						ExecutionUtility.DoExecutionByCut(this.pawn, base.Victim);
						this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
						if (this.pawn.InMentalState)
						{
							this.pawn.MentalState.Notify_SlaughteredAnimal();
						}
					});
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				case 12u:
					this.$current = Toils_Jump.Jump(startCollectCorpseLabel);
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				case 13u:
					this.$current = startCollectCorpseLabel;
					if (!this.$disposing)
					{
						this.$PC = 14;
					}
					return true;
				case 14u:
					this.$current = base.StartCollectCorpseToil();
					if (!this.$disposing)
					{
						this.$PC = 15;
					}
					return true;
				case 15u:
					this.$current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 16;
					}
					return true;
				case 16u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
					if (!this.$disposing)
					{
						this.$PC = 17;
					}
					return true;
				case 17u:
					carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
					this.$current = carryToCell;
					if (!this.$disposing)
					{
						this.$PC = 18;
					}
					return true;
				case 18u:
					this.$current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
					if (!this.$disposing)
					{
						this.$PC = 19;
					}
					return true;
				case 19u:
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
				JobDriver_Hunt.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Hunt.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				if (!this.job.ignoreDesignations)
				{
					Pawn victim = base.Victim;
					if (victim != null && !victim.Dead && base.Map.designationManager.DesignationOn(victim, DesignationDefOf.Hunt) == null)
					{
						return true;
					}
				}
				return false;
			}

			internal void <>m__1()
			{
				this.jobStartTick = Find.TickManager.TicksGame;
			}

			internal bool <>m__2()
			{
				return Find.TickManager.TicksGame > this.jobStartTick + 5000;
			}

			internal bool <>m__3()
			{
				Pawn victim = base.Victim;
				return (victim.RaceProps.DeathActionWorker == null || !victim.RaceProps.DeathActionWorker.DangerousInMelee) && victim.Downed;
			}

			internal bool <>m__4()
			{
				return Find.TickManager.TicksGame > this.jobStartTick + 5000;
			}

			internal void <>m__5()
			{
				if (base.Victim.Dead)
				{
					return;
				}
				ExecutionUtility.DoExecutionByCut(this.pawn, base.Victim);
				this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				if (this.pawn.InMentalState)
				{
					this.pawn.MentalState.Notify_SlaughteredAnimal();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <StartCollectCorpseToil>c__AnonStorey1
		{
			internal Toil toil;

			internal JobDriver_Hunt $this;

			public <StartCollectCorpseToil>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				if (this.$this.Victim == null)
				{
					this.toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.Hunted, new object[]
				{
					this.$this.pawn,
					this.$this.Victim
				});
				Corpse corpse = this.$this.Victim.Corpse;
				if (corpse == null || !this.$this.pawn.CanReserveAndReach(corpse, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
				{
					this.$this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					return;
				}
				corpse.SetForbidden(false, true);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(corpse, this.$this.pawn, this.$this.Map, StoragePriority.Unstored, this.$this.pawn.Faction, out c, true))
				{
					this.$this.pawn.Reserve(corpse, this.$this.job, 1, -1, null, true);
					this.$this.pawn.Reserve(c, this.$this.job, 1, -1, null, true);
					this.$this.job.SetTarget(TargetIndex.B, c);
					this.$this.job.SetTarget(TargetIndex.A, corpse);
					this.$this.job.count = 1;
					this.$this.job.haulMode = HaulMode.ToCellStorage;
					return;
				}
				this.$this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
			}
		}
	}
}
