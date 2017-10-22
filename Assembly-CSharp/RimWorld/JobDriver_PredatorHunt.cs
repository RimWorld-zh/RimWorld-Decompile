using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PredatorHunt : JobDriver
	{
		private bool notifiedPlayer;

		private bool firstHit = true;

		public const TargetIndex PreyInd = TargetIndex.A;

		private const TargetIndex CorpseInd = TargetIndex.A;

		private const int MaxHuntTicks = 5000;

		public Pawn Prey
		{
			get
			{
				Corpse corpse = this.Corpse;
				return (corpse == null) ? ((Pawn)base.job.GetTarget(TargetIndex.A).Thing) : corpse.InnerPawn;
			}
		}

		private Corpse Corpse
		{
			get
			{
				return base.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.firstHit, "firstHit", false, false);
		}

		public override string GetReport()
		{
			return (this.Corpse == null) ? base.GetReport() : base.ReportStringProcessed(JobDefOf.Ingest.reportString);
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/;
			base.AddFinishAction((Action)delegate
			{
				_003CMakeNewToils_003Ec__Iterator._0024this.Map.attackTargetsCache.UpdateTarget(_003CMakeNewToils_003Ec__Iterator._0024this.pawn);
			});
			Toil prepareToEatCorpse = new Toil();
			prepareToEatCorpse.initAction = (Action)delegate
			{
				Pawn actor = prepareToEatCorpse.actor;
				Corpse corpse = _003CMakeNewToils_003Ec__Iterator._0024this.Corpse;
				if (corpse == null)
				{
					Pawn prey = _003CMakeNewToils_003Ec__Iterator._0024this.Prey;
					if (prey == null)
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					else
					{
						corpse = prey.Corpse;
						if (corpse != null && corpse.Spawned)
						{
							goto IL_007b;
						}
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					return;
				}
				goto IL_007b;
				IL_007b:
				if (actor.Faction == Faction.OfPlayer)
				{
					corpse.SetForbidden(false, false);
				}
				else
				{
					corpse.SetForbidden(true, false);
				}
				actor.CurJob.SetTarget(TargetIndex.A, (Thing)corpse);
			};
			yield return Toils_General.DoAtomic((Action)delegate
			{
				_003CMakeNewToils_003Ec__Iterator._0024this.Map.attackTargetsCache.UpdateTarget(_003CMakeNewToils_003Ec__Iterator._0024this.pawn);
			});
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
