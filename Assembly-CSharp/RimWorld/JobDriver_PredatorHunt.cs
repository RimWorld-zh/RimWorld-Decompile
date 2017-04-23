using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PredatorHunt : JobDriver
	{
		public const TargetIndex PreyInd = TargetIndex.A;

		private const TargetIndex CorpseInd = TargetIndex.A;

		private const int MaxHuntTicks = 5000;

		private bool notifiedPlayer;

		private bool firstHit = true;

		public Pawn Prey
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
			Scribe_Values.Look<bool>(ref this.firstHit, "firstHit", false, false);
		}

		public override string GetReport()
		{
			if (this.Corpse != null)
			{
				return base.ReportStringProcessed(JobDefOf.Ingest.reportString);
			}
			return base.GetReport();
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PredatorHunt.<MakeNewToils>c__Iterator3 <MakeNewToils>c__Iterator = new JobDriver_PredatorHunt.<MakeNewToils>c__Iterator3();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_PredatorHunt.<MakeNewToils>c__Iterator3 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
