using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000030 RID: 48
	public class JobDriver_PredatorHunt : JobDriver
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000127D8 File Offset: 0x00010BD8
		public Pawn Prey
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

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00012820 File Offset: 0x00010C20
		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0001284E File Offset: 0x00010C4E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.firstHit, "firstHit", false, false);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0001286C File Offset: 0x00010C6C
		public override string GetReport()
		{
			string result;
			if (this.Corpse != null)
			{
				result = base.ReportStringProcessed(JobDefOf.Ingest.reportString);
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000128A8 File Offset: 0x00010CA8
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000128C0 File Offset: 0x00010CC0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddFinishAction(delegate
			{
				this.Map.attackTargetsCache.UpdateTarget(this.pawn);
			});
			Toil prepareToEatCorpse = new Toil();
			prepareToEatCorpse.initAction = delegate()
			{
				Pawn actor = prepareToEatCorpse.actor;
				Corpse corpse = this.Corpse;
				if (corpse == null)
				{
					Pawn prey = this.Prey;
					if (prey == null)
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
						return;
					}
					corpse = prey.Corpse;
					if (corpse == null || !corpse.Spawned)
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
				actor.CurJob.SetTarget(TargetIndex.A, corpse);
			};
			yield return Toils_General.DoAtomic(delegate
			{
				this.Map.attackTargetsCache.UpdateTarget(this.pawn);
			});
			Action onHitAction = delegate()
			{
				Pawn prey = this.Prey;
				bool surpriseAttack = this.firstHit && !prey.IsColonist;
				if (this.pawn.meleeVerbs.TryMeleeAttack(prey, this.job.verbToUse, surpriseAttack))
				{
					if (!this.notifiedPlayer && PawnUtility.ShouldSendNotificationAbout(prey))
					{
						this.notifiedPlayer = true;
						Messages.Message("MessageAttackedByPredator".Translate(new object[]
						{
							prey.LabelShort,
							this.pawn.LabelIndefinite()
						}).CapitalizeFirst(), prey, MessageTypeDefOf.ThreatSmall, true);
					}
					this.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
				this.firstHit = false;
			};
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, onHitAction).JumpIfDespawnedOrNull(TargetIndex.A, prepareToEatCorpse).JumpIf(() => this.Corpse != null, prepareToEatCorpse).FailOn(() => Find.TickManager.TicksGame > this.startTick + 5000 && (float)(this.job.GetTarget(TargetIndex.A).Cell - this.pawn.Position).LengthHorizontalSquared > 4f);
			yield return prepareToEatCorpse;
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return gotoCorpse;
			float durationMultiplier = 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true);
			yield return Toils_Ingest.ChewIngestible(this.pawn, durationMultiplier, TargetIndex.A, TargetIndex.None).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(gotoCorpse, () => this.pawn.needs.food.CurLevelPercentage < 0.9f);
			yield break;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000128EC File Offset: 0x00010CEC
		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.externalViolence && dinfo.Def.isRanged && dinfo.Instigator != null && dinfo.Instigator != this.Prey && !this.pawn.InMentalState && !this.pawn.Downed)
			{
				this.pawn.mindState.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
			}
		}

		// Token: 0x040001B1 RID: 433
		private bool notifiedPlayer;

		// Token: 0x040001B2 RID: 434
		private bool firstHit = true;

		// Token: 0x040001B3 RID: 435
		public const TargetIndex PreyInd = TargetIndex.A;

		// Token: 0x040001B4 RID: 436
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x040001B5 RID: 437
		private const int MaxHuntTicks = 5000;
	}
}
