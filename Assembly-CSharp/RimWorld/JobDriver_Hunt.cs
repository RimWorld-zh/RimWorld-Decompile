using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Hunt : JobDriver
	{
		private const TargetIndex VictimInd = TargetIndex.A;

		private const TargetIndex CorpseInd = TargetIndex.A;

		private const TargetIndex StoreCellInd = TargetIndex.B;

		private const int MaxHuntTicks = 5000;

		private int jobStartTick = -1;

		public Pawn Victim
		{
			get
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Corpse Corpse
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jobStartTick, "jobStartTick", 0, false);
		}

		public override string GetReport()
		{
			return base.CurJob.def.reportString.Replace("TargetA", this.Victim.LabelShort);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)delegate
			{
				if (!((_003CMakeNewToils_003Ec__Iterator2E)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.CurJob.ignoreDesignations)
				{
					Pawn victim = ((_003CMakeNewToils_003Ec__Iterator2E)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.Victim;
					if (victim != null && !victim.Dead && ((_003CMakeNewToils_003Ec__Iterator2E)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.Map.designationManager.DesignationOn(victim, DesignationDefOf.Hunt) == null)
					{
						return true;
					}
				}
				return false;
			});
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator2E)/*Error near IL_0099: stateMachine*/)._003C_003Ef__this.jobStartTick = Find.TickManager.TicksGame;
				}
			};
			yield return Toils_Combat.TrySetJobToUseAttackVerb();
			Toil startCollectCorpse = this.StartCollectCorpseToil();
			Toil gotoCastPos = Toils_Combat.GotoCastPosition(TargetIndex.A, true).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpse).FailOn((Func<bool>)(() => Find.TickManager.TicksGame > ((_003CMakeNewToils_003Ec__Iterator2E)/*Error near IL_00fe: stateMachine*/)._003C_003Ef__this.jobStartTick + 5000));
			yield return gotoCastPos;
			Toil moveIfCannotHit = Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, gotoCastPos);
			yield return moveIfCannotHit;
			yield return Toils_Jump.JumpIfTargetDownedDistant(TargetIndex.A, gotoCastPos);
			yield return Toils_Combat.CastVerb(TargetIndex.A, false).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpse).FailOn((Func<bool>)(() => Find.TickManager.TicksGame > ((_003CMakeNewToils_003Ec__Iterator2E)/*Error near IL_0188: stateMachine*/)._003C_003Ef__this.jobStartTick + 5000));
			yield return Toils_Jump.JumpIfTargetDespawnedOrNull(TargetIndex.A, startCollectCorpse);
			yield return Toils_Jump.Jump(moveIfCannotHit);
			yield return startCollectCorpse;
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
		}

		private Toil StartCollectCorpseToil()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
			{
				if (this.Victim == null)
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					TaleRecorder.RecordTale(TaleDefOf.Hunted, base.pawn, this.Victim);
					Corpse corpse = this.Victim.Corpse;
					if (corpse == null || !base.pawn.CanReserveAndReach((Thing)corpse, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
					{
						base.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					else
					{
						corpse.SetForbidden(false, true);
						IntVec3 c = default(IntVec3);
						if (StoreUtility.TryFindBestBetterStoreCellFor((Thing)corpse, base.pawn, base.Map, StoragePriority.Unstored, base.pawn.Faction, out c, true))
						{
							base.pawn.Reserve((Thing)corpse, 1, -1, null);
							base.pawn.Reserve(c, 1, -1, null);
							base.pawn.CurJob.SetTarget(TargetIndex.B, c);
							base.pawn.CurJob.SetTarget(TargetIndex.A, (Thing)corpse);
							base.pawn.CurJob.count = 1;
							base.pawn.CurJob.haulMode = HaulMode.ToCellStorage;
						}
						else
						{
							base.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
						}
					}
				}
			};
			return toil;
		}
	}
}
