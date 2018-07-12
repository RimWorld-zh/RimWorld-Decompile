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
	public class JobDriver_PredatorHunt : JobDriver
	{
		private bool notifiedPlayerAttacked;

		private bool notifiedPlayerAttacking;

		private bool firstHit = true;

		public const TargetIndex PreyInd = TargetIndex.A;

		private const TargetIndex CorpseInd = TargetIndex.A;

		private const int MaxHuntTicks = 5000;

		public JobDriver_PredatorHunt()
		{
		}

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
			Scribe_Values.Look<bool>(ref this.firstHit, "firstHit", false, false);
			Scribe_Values.Look<bool>(ref this.notifiedPlayerAttacking, "notifiedPlayerAttacking", false, false);
		}

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

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

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
					if (!this.notifiedPlayerAttacked && PawnUtility.ShouldSendNotificationAbout(prey))
					{
						this.notifiedPlayerAttacked = true;
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
			Toil followAndAttack = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, onHitAction).JumpIfDespawnedOrNull(TargetIndex.A, prepareToEatCorpse).JumpIf(() => this.Corpse != null, prepareToEatCorpse).FailOn(() => Find.TickManager.TicksGame > this.startTick + 5000 && (float)(this.job.GetTarget(TargetIndex.A).Cell - this.pawn.Position).LengthHorizontalSquared > 4f);
			followAndAttack.AddPreTickAction(new Action(this.CheckWarnPlayer));
			yield return followAndAttack;
			yield return prepareToEatCorpse;
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return gotoCorpse;
			float durationMultiplier = 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true);
			yield return Toils_Ingest.ChewIngestible(this.pawn, durationMultiplier, TargetIndex.A, TargetIndex.None).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(gotoCorpse, () => this.pawn.needs.food.CurLevelPercentage < 0.9f);
			yield break;
		}

		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.externalViolence && dinfo.Def.isRanged && dinfo.Instigator != null && dinfo.Instigator != this.Prey && !this.pawn.InMentalState && !this.pawn.Downed)
			{
				this.pawn.mindState.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
			}
		}

		private void CheckWarnPlayer()
		{
			if (!this.notifiedPlayerAttacking)
			{
				Pawn prey = this.Prey;
				if (prey.Spawned && prey.Faction == Faction.OfPlayer)
				{
					if (Find.TickManager.TicksGame > this.pawn.mindState.lastPredatorHuntingPlayerNotificationTick + 2500)
					{
						if (prey.Position.InHorDistOf(this.pawn.Position, 60f))
						{
							if (prey.RaceProps.Humanlike)
							{
								Find.LetterStack.ReceiveLetter("LetterLabelPredatorHuntingColonist".Translate(new object[]
								{
									this.pawn.LabelShort,
									prey.LabelDefinite()
								}).CapitalizeFirst(), "LetterPredatorHuntingColonist".Translate(new object[]
								{
									this.pawn.LabelIndefinite(),
									prey.LabelDefinite()
								}).CapitalizeFirst(), LetterDefOf.ThreatBig, this.pawn, null, null);
							}
							else
							{
								Messages.Message("MessagePredatorHuntingPlayerAnimal".Translate(new object[]
								{
									this.pawn.LabelIndefinite(),
									prey.LabelDefinite()
								}).CapitalizeFirst(), this.pawn, MessageTypeDefOf.ThreatBig, true);
							}
							this.pawn.mindState.Notify_PredatorHuntingPlayerNotification();
							this.notifiedPlayerAttacking = true;
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Action <onHitAction>__0;

			internal Toil <followAndAttack>__0;

			internal Toil <gotoCorpse>__0;

			internal float <durationMultiplier>__0;

			internal JobDriver_PredatorHunt $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_PredatorHunt.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

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
					this.$current = Toils_General.DoAtomic(delegate
					{
						this.Map.attackTargetsCache.UpdateTarget(this.pawn);
					});
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
					onHitAction = delegate()
					{
						Pawn prey = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Prey;
						bool surpriseAttack = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.firstHit && !prey.IsColonist;
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.meleeVerbs.TryMeleeAttack(prey, <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.verbToUse, surpriseAttack))
						{
							if (!<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.notifiedPlayerAttacked && PawnUtility.ShouldSendNotificationAbout(prey))
							{
								<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.notifiedPlayerAttacked = true;
								Messages.Message("MessageAttackedByPredator".Translate(new object[]
								{
									prey.LabelShort,
									<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.LabelIndefinite()
								}).CapitalizeFirst(), prey, MessageTypeDefOf.ThreatSmall, true);
							}
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map.attackTargetsCache.UpdateTarget(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn);
						}
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.firstHit = false;
					};
					followAndAttack = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, onHitAction).JumpIfDespawnedOrNull(TargetIndex.A, <MakeNewToils>c__AnonStorey.prepareToEatCorpse).JumpIf(() => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Corpse != null, <MakeNewToils>c__AnonStorey.prepareToEatCorpse).FailOn(() => Find.TickManager.TicksGame > <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.startTick + 5000 && (float)(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.job.GetTarget(TargetIndex.A).Cell - <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.Position).LengthHorizontalSquared > 4f);
					followAndAttack.AddPreTickAction(new Action(base.CheckWarnPlayer));
					this.$current = followAndAttack;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = <MakeNewToils>c__AnonStorey.prepareToEatCorpse;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					this.$current = gotoCorpse;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					durationMultiplier = 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true);
					this.$current = Toils_Ingest.ChewIngestible(this.pawn, durationMultiplier, TargetIndex.A, TargetIndex.None).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Jump.JumpIf(gotoCorpse, () => <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.needs.food.CurLevelPercentage < 0.9f);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
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
				JobDriver_PredatorHunt.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_PredatorHunt.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil prepareToEatCorpse;

				internal JobDriver_PredatorHunt.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.Map.attackTargetsCache.UpdateTarget(this.<>f__ref$0.$this.pawn);
				}

				internal void <>m__1()
				{
					Pawn actor = this.prepareToEatCorpse.actor;
					Corpse corpse = this.<>f__ref$0.$this.Corpse;
					if (corpse == null)
					{
						Pawn prey = this.<>f__ref$0.$this.Prey;
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
				}

				internal void <>m__2()
				{
					this.<>f__ref$0.$this.Map.attackTargetsCache.UpdateTarget(this.<>f__ref$0.$this.pawn);
				}

				internal void <>m__3()
				{
					Pawn prey = this.<>f__ref$0.$this.Prey;
					bool surpriseAttack = this.<>f__ref$0.$this.firstHit && !prey.IsColonist;
					if (this.<>f__ref$0.$this.pawn.meleeVerbs.TryMeleeAttack(prey, this.<>f__ref$0.$this.job.verbToUse, surpriseAttack))
					{
						if (!this.<>f__ref$0.$this.notifiedPlayerAttacked && PawnUtility.ShouldSendNotificationAbout(prey))
						{
							this.<>f__ref$0.$this.notifiedPlayerAttacked = true;
							Messages.Message("MessageAttackedByPredator".Translate(new object[]
							{
								prey.LabelShort,
								this.<>f__ref$0.$this.pawn.LabelIndefinite()
							}).CapitalizeFirst(), prey, MessageTypeDefOf.ThreatSmall, true);
						}
						this.<>f__ref$0.$this.Map.attackTargetsCache.UpdateTarget(this.<>f__ref$0.$this.pawn);
					}
					this.<>f__ref$0.$this.firstHit = false;
				}

				internal bool <>m__4()
				{
					return this.<>f__ref$0.$this.Corpse != null;
				}

				internal bool <>m__5()
				{
					return Find.TickManager.TicksGame > this.<>f__ref$0.$this.startTick + 5000 && (float)(this.<>f__ref$0.$this.job.GetTarget(TargetIndex.A).Cell - this.<>f__ref$0.$this.pawn.Position).LengthHorizontalSquared > 4f;
				}

				internal bool <>m__6()
				{
					return this.<>f__ref$0.$this.pawn.needs.food.CurLevelPercentage < 0.9f;
				}
			}
		}
	}
}
