using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class Toils_Interpersonal
	{
		public static Toil GotoInteractablePosition(TargetIndex target)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				if (InteractionUtility.IsGoodPositionForInteraction(actor, pawn))
				{
					actor.jobs.curDriver.ReadyForNextToil();
				}
				else
				{
					actor.pather.StartPath(pawn, PathEndMode.Touch);
				}
			};
			toil.tickAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				Map map = actor.Map;
				if (InteractionUtility.IsGoodPositionForInteraction(actor, pawn) && actor.Position.InHorDistOf(pawn.Position, (float)Mathf.CeilToInt(3f)) && (!actor.pather.Moving || actor.pather.nextCell.GetDoor(map) == null))
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
				}
				else if (!actor.pather.Moving)
				{
					IntVec3 intVec = IntVec3.Invalid;
					for (int i = 0; i < 8; i++)
					{
						IntVec3 intVec2 = pawn.Position + GenAdj.AdjacentCells[i];
						if (intVec2.InBounds(map) && intVec2.Standable(map) && intVec2 != actor.Position && InteractionUtility.IsGoodPositionForInteraction(intVec2, pawn.Position, map) && actor.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn) && (!intVec.IsValid || actor.Position.DistanceToSquared(intVec2) < actor.Position.DistanceToSquared(intVec)))
						{
							intVec = intVec2;
						}
					}
					if (intVec.IsValid)
					{
						actor.pather.StartPath(intVec, PathEndMode.OnCell);
					}
					else
					{
						actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					}
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		public static Toil GotoPrisoner(Pawn pawn, Pawn talkee, PrisonerInteractionModeDef mode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || (mode != PrisonerInteractionModeDefOf.Execution && !talkee.Awake()) || !talkee.IsPrisonerOfColony || (talkee.guest == null || talkee.guest.interactionMode != mode));
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		public static Toil WaitToBeAbleToInteract(Pawn pawn)
		{
			return new Toil
			{
				initAction = delegate()
				{
					if (!pawn.interactions.InteractedTooRecentlyToInteract())
					{
						pawn.jobs.curDriver.ReadyForNextToil();
					}
				},
				tickAction = delegate()
				{
					if (!pawn.interactions.InteractedTooRecentlyToInteract())
					{
						pawn.jobs.curDriver.ReadyForNextToil();
					}
				},
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Never
			};
		}

		public static Toil ConvinceRecruitee(Pawn pawn, Pawn talkee)
		{
			return new Toil
			{
				initAction = delegate()
				{
					if (!pawn.interactions.TryInteractWith(talkee, InteractionDefOf.BuildRapport))
					{
						pawn.jobs.curDriver.ReadyForNextToil();
					}
					else
					{
						pawn.records.Increment(RecordDefOf.PrisonersChatted);
					}
				},
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 350
			};
		}

		public static Toil SetLastInteractTime(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(targetInd).Thing;
				pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
				pawn.mindState.interactionsToday++;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		public static Toil TryRecruit(TargetIndex recruiteeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(recruiteeInd).Thing;
				if (!pawn.Spawned || !pawn.Awake())
				{
					return;
				}
				InteractionDef intDef = (!pawn.AnimalOrWildMan()) ? InteractionDefOf.RecruitAttempt : InteractionDefOf.TameAttempt;
				actor.interactions.TryInteractWith(pawn, intDef);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			return toil;
		}

		public static Toil TryTrain(TargetIndex traineeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(traineeInd).Thing;
				if (pawn.Spawned && pawn.Awake() && actor.interactions.TryInteractWith(pawn, InteractionDefOf.TrainAttempt))
				{
					float num = actor.GetStatValue(StatDefOf.TrainAnimalChance, true);
					num *= GenMath.LerpDouble(0f, 1f, 1.5f, 0.5f, pawn.RaceProps.wildness);
					if (actor.relations.DirectRelationExists(PawnRelationDefOf.Bond, pawn))
					{
						num *= 5f;
					}
					num = Mathf.Clamp01(num);
					TrainableDef trainableDef = pawn.training.NextTrainableToTrain();
					if (trainableDef == null)
					{
						Log.ErrorOnce("Attempted to train untrainable animal", 7842936, false);
						return;
					}
					string text;
					if (Rand.Value < num)
					{
						pawn.training.Train(trainableDef, actor, false);
						if (pawn.caller != null)
						{
							pawn.caller.DoCall();
						}
						text = "TextMote_TrainSuccess".Translate(new object[]
						{
							trainableDef.LabelCap,
							num.ToStringPercent()
						});
						RelationsUtility.TryDevelopBondRelation(actor, pawn, 0.007f);
						TaleRecorder.RecordTale(TaleDefOf.TrainedAnimal, new object[]
						{
							actor,
							pawn,
							trainableDef
						});
					}
					else
					{
						text = "TextMote_TrainFail".Translate(new object[]
						{
							trainableDef.LabelCap,
							num.ToStringPercent()
						});
					}
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"\n",
						pawn.training.GetSteps(trainableDef),
						" / ",
						trainableDef.steps
					});
					MoteMaker.ThrowText((actor.DrawPos + pawn.DrawPos) / 2f, actor.Map, text, 5f);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 100;
			return toil;
		}

		public static Toil Interact(TargetIndex otherPawnInd, InteractionDef interaction)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(otherPawnInd).Thing;
				if (!pawn.Spawned)
				{
					return;
				}
				actor.interactions.TryInteractWith(pawn, interaction);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 60;
			return toil;
		}

		[CompilerGenerated]
		private sealed class <GotoInteractablePosition>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex target;

			public <GotoInteractablePosition>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(this.target));
				if (InteractionUtility.IsGoodPositionForInteraction(actor, pawn))
				{
					actor.jobs.curDriver.ReadyForNextToil();
				}
				else
				{
					actor.pather.StartPath(pawn, PathEndMode.Touch);
				}
			}

			internal void <>m__1()
			{
				Pawn actor = this.toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(this.target));
				Map map = actor.Map;
				if (InteractionUtility.IsGoodPositionForInteraction(actor, pawn) && actor.Position.InHorDistOf(pawn.Position, (float)Mathf.CeilToInt(3f)) && (!actor.pather.Moving || actor.pather.nextCell.GetDoor(map) == null))
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
				}
				else if (!actor.pather.Moving)
				{
					IntVec3 intVec = IntVec3.Invalid;
					for (int i = 0; i < 8; i++)
					{
						IntVec3 intVec2 = pawn.Position + GenAdj.AdjacentCells[i];
						if (intVec2.InBounds(map) && intVec2.Standable(map) && intVec2 != actor.Position && InteractionUtility.IsGoodPositionForInteraction(intVec2, pawn.Position, map) && actor.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn) && (!intVec.IsValid || actor.Position.DistanceToSquared(intVec2) < actor.Position.DistanceToSquared(intVec)))
						{
							intVec = intVec2;
						}
					}
					if (intVec.IsValid)
					{
						actor.pather.StartPath(intVec, PathEndMode.OnCell);
					}
					else
					{
						actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GotoPrisoner>c__AnonStorey1
		{
			internal Pawn pawn;

			internal Pawn talkee;

			internal PrisonerInteractionModeDef mode;

			public <GotoPrisoner>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.pawn.pather.StartPath(this.talkee, PathEndMode.Touch);
			}

			internal bool <>m__1()
			{
				return this.talkee.DestroyedOrNull() || (this.mode != PrisonerInteractionModeDefOf.Execution && !this.talkee.Awake()) || !this.talkee.IsPrisonerOfColony || (this.talkee.guest == null || this.talkee.guest.interactionMode != this.mode);
			}
		}

		[CompilerGenerated]
		private sealed class <WaitToBeAbleToInteract>c__AnonStorey2
		{
			internal Pawn pawn;

			public <WaitToBeAbleToInteract>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				if (!this.pawn.interactions.InteractedTooRecentlyToInteract())
				{
					this.pawn.jobs.curDriver.ReadyForNextToil();
				}
			}

			internal void <>m__1()
			{
				if (!this.pawn.interactions.InteractedTooRecentlyToInteract())
				{
					this.pawn.jobs.curDriver.ReadyForNextToil();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ConvinceRecruitee>c__AnonStorey3
		{
			internal Pawn pawn;

			internal Pawn talkee;

			public <ConvinceRecruitee>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				if (!this.pawn.interactions.TryInteractWith(this.talkee, InteractionDefOf.BuildRapport))
				{
					this.pawn.jobs.curDriver.ReadyForNextToil();
				}
				else
				{
					this.pawn.records.Increment(RecordDefOf.PrisonersChatted);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <SetLastInteractTime>c__AnonStorey4
		{
			internal Toil toil;

			internal TargetIndex targetInd;

			public <SetLastInteractTime>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				Pawn pawn = (Pawn)this.toil.actor.jobs.curJob.GetTarget(this.targetInd).Thing;
				pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
				pawn.mindState.interactionsToday++;
			}
		}

		[CompilerGenerated]
		private sealed class <TryRecruit>c__AnonStorey5
		{
			internal Toil toil;

			internal TargetIndex recruiteeInd;

			public <TryRecruit>c__AnonStorey5()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(this.recruiteeInd).Thing;
				if (!pawn.Spawned || !pawn.Awake())
				{
					return;
				}
				InteractionDef intDef = (!pawn.AnimalOrWildMan()) ? InteractionDefOf.RecruitAttempt : InteractionDefOf.TameAttempt;
				actor.interactions.TryInteractWith(pawn, intDef);
			}
		}

		[CompilerGenerated]
		private sealed class <TryTrain>c__AnonStorey6
		{
			internal Toil toil;

			internal TargetIndex traineeInd;

			public <TryTrain>c__AnonStorey6()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(this.traineeInd).Thing;
				if (pawn.Spawned && pawn.Awake() && actor.interactions.TryInteractWith(pawn, InteractionDefOf.TrainAttempt))
				{
					float num = actor.GetStatValue(StatDefOf.TrainAnimalChance, true);
					num *= GenMath.LerpDouble(0f, 1f, 1.5f, 0.5f, pawn.RaceProps.wildness);
					if (actor.relations.DirectRelationExists(PawnRelationDefOf.Bond, pawn))
					{
						num *= 5f;
					}
					num = Mathf.Clamp01(num);
					TrainableDef trainableDef = pawn.training.NextTrainableToTrain();
					if (trainableDef == null)
					{
						Log.ErrorOnce("Attempted to train untrainable animal", 7842936, false);
						return;
					}
					string text;
					if (Rand.Value < num)
					{
						pawn.training.Train(trainableDef, actor, false);
						if (pawn.caller != null)
						{
							pawn.caller.DoCall();
						}
						text = "TextMote_TrainSuccess".Translate(new object[]
						{
							trainableDef.LabelCap,
							num.ToStringPercent()
						});
						RelationsUtility.TryDevelopBondRelation(actor, pawn, 0.007f);
						TaleRecorder.RecordTale(TaleDefOf.TrainedAnimal, new object[]
						{
							actor,
							pawn,
							trainableDef
						});
					}
					else
					{
						text = "TextMote_TrainFail".Translate(new object[]
						{
							trainableDef.LabelCap,
							num.ToStringPercent()
						});
					}
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"\n",
						pawn.training.GetSteps(trainableDef),
						" / ",
						trainableDef.steps
					});
					MoteMaker.ThrowText((actor.DrawPos + pawn.DrawPos) / 2f, actor.Map, text, 5f);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <Interact>c__AnonStorey7
		{
			internal Toil toil;

			internal TargetIndex otherPawnInd;

			internal InteractionDef interaction;

			public <Interact>c__AnonStorey7()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(this.otherPawnInd).Thing;
				if (!pawn.Spawned)
				{
					return;
				}
				actor.interactions.TryInteractWith(pawn, this.interaction);
			}
		}
	}
}
