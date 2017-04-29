using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Hunt.<MakeNewToils>c__Iterator2E <MakeNewToils>c__Iterator2E = new JobDriver_Hunt.<MakeNewToils>c__Iterator2E();
			<MakeNewToils>c__Iterator2E.<>f__this = this;
			JobDriver_Hunt.<MakeNewToils>c__Iterator2E expr_0E = <MakeNewToils>c__Iterator2E;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private Toil StartCollectCorpseToil()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
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
					this.pawn.Reserve(corpse, 1, -1, null);
					this.pawn.Reserve(c, 1, -1, null);
					this.pawn.CurJob.SetTarget(TargetIndex.B, c);
					this.pawn.CurJob.SetTarget(TargetIndex.A, corpse);
					this.pawn.CurJob.count = 1;
					this.pawn.CurJob.haulMode = HaulMode.ToCellStorage;
					return;
				}
				this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
			};
			return toil;
		}
	}
}
