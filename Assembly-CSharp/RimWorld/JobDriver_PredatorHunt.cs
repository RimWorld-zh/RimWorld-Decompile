using System;
using System.Collections.Generic;
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

		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddFinishAction((Action)delegate
			{
				((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_003f: stateMachine*/)._003C_003Ef__this.Map.attackTargetsCache.UpdateTarget(((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_003f: stateMachine*/)._003C_003Ef__this.pawn);
			});
			Toil prepareToEatCorpse = new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_0061: stateMachine*/)._003CprepareToEatCorpse_003E__0.actor;
					Corpse corpse = ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_0061: stateMachine*/)._003C_003Ef__this.Corpse;
					if (corpse == null)
					{
						Pawn prey2 = ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_0061: stateMachine*/)._003C_003Ef__this.Prey;
						if (prey2 == null)
						{
							actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
							return;
						}
						corpse = prey2.Corpse;
						if (corpse == null)
						{
							actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
							return;
						}
					}
					if (actor.Faction == Faction.OfPlayer)
					{
						corpse.SetForbidden(false, false);
					}
					else
					{
						corpse.SetForbidden(true, false);
					}
					actor.CurJob.SetTarget(TargetIndex.A, (Thing)corpse);
				}
			};
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_0083: stateMachine*/)._003C_003Ef__this.Map.attackTargetsCache.UpdateTarget(((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_0083: stateMachine*/)._003C_003Ef__this.pawn);
				},
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Action onHitAction = (Action)delegate
			{
				Pawn prey = ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.Prey;
				bool surpriseAttack = ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.firstHit && !prey.IsColonist;
				if (((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.pawn.meleeVerbs.TryMeleeAttack(prey, ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.CurJob.verbToUse, surpriseAttack))
				{
					if (!((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.notifiedPlayer && PawnUtility.ShouldSendNotificationAbout(prey))
					{
						((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.notifiedPlayer = true;
						Messages.Message("MessageAttackedByPredator".Translate(prey.LabelShort, ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.pawn.LabelIndefinite()).CapitalizeFirst(), (Thing)prey, MessageSound.SeriousAlert);
					}
					((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.Map.attackTargetsCache.UpdateTarget(((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.pawn);
				}
				((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00c5: stateMachine*/)._003C_003Ef__this.firstHit = false;
			};
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, onHitAction).JumpIfDespawnedOrNull(TargetIndex.A, prepareToEatCorpse).JumpIf((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_00ef: stateMachine*/)._003C_003Ef__this.Corpse != null), prepareToEatCorpse).FailOn((Func<bool>)(() => Find.TickManager.TicksGame > ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_0106: stateMachine*/)._003C_003Ef__this.startTick + 5000 && (float)(((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_0106: stateMachine*/)._003C_003Ef__this.CurJob.GetTarget(TargetIndex.A).Cell - ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_0106: stateMachine*/)._003C_003Ef__this.pawn.Position).LengthHorizontalSquared > 4.0));
			yield return prepareToEatCorpse;
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return gotoCorpse;
			float durationMultiplier = (float)(1.0 / base.pawn.GetStatValue(StatDefOf.EatingSpeed, true));
			yield return Toils_Ingest.ChewIngestible(base.pawn, durationMultiplier, TargetIndex.A, TargetIndex.None).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(base.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(gotoCorpse, (Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator3)/*Error near IL_01e8: stateMachine*/)._003C_003Ef__this.pawn.needs.food.CurLevelPercentage < 0.89999997615814209));
		}
	}
}
