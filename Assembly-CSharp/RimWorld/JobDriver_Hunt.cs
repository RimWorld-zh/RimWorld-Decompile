using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200006D RID: 109
	public class JobDriver_Hunt : JobDriver
	{
		// Token: 0x04000213 RID: 531
		private int jobStartTick = -1;

		// Token: 0x04000214 RID: 532
		private const TargetIndex VictimInd = TargetIndex.A;

		// Token: 0x04000215 RID: 533
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04000216 RID: 534
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x04000217 RID: 535
		private const int MaxHuntTicks = 5000;

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000303 RID: 771 RVA: 0x000206F4 File Offset: 0x0001EAF4
		public Pawn Victim
		{
			get
			{
				Corpse corpse = this.Corpse;
				Pawn result;
				if (corpse != null)
				{
					result = corpse.InnerPawn;
				}
				else
				{
					result = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				}
				return result;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0002073C File Offset: 0x0001EB3C
		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0002076A File Offset: 0x0001EB6A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jobStartTick, "jobStartTick", 0, false);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00020788 File Offset: 0x0001EB88
		public override string GetReport()
		{
			string result;
			if (this.Victim != null)
			{
				result = this.job.def.reportString.Replace("TargetA", this.Victim.LabelShort);
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x000207DC File Offset: 0x0001EBDC
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00020810 File Offset: 0x0001EC10
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
				if (!this.Victim.Dead)
				{
					ExecutionUtility.DoExecutionByCut(this.pawn, this.Victim);
					this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
					if (this.pawn.InMentalState)
					{
						this.pawn.MentalState.Notify_SlaughteredAnimal();
					}
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

		// Token: 0x06000309 RID: 777 RVA: 0x0002083C File Offset: 0x0001EC3C
		private Toil StartCollectCorpseToil()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (this.Victim == null)
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					TaleRecorder.RecordTale(TaleDefOf.Hunted, new object[]
					{
						this.pawn,
						this.Victim
					});
					Corpse corpse = this.Victim.Corpse;
					if (corpse == null || !this.pawn.CanReserveAndReach(corpse, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					else
					{
						corpse.SetForbidden(false, true);
						IntVec3 c;
						if (StoreUtility.TryFindBestBetterStoreCellFor(corpse, this.pawn, this.Map, StoragePriority.Unstored, this.pawn.Faction, out c, true))
						{
							this.pawn.Reserve(corpse, this.job, 1, -1, null);
							this.pawn.Reserve(c, this.job, 1, -1, null);
							this.job.SetTarget(TargetIndex.B, c);
							this.job.SetTarget(TargetIndex.A, corpse);
							this.job.count = 1;
							this.job.haulMode = HaulMode.ToCellStorage;
						}
						else
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
						}
					}
				}
			};
			return toil;
		}
	}
}
