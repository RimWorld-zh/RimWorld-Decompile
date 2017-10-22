using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class Toils_Interpersonal
	{
		public static Toil GotoPrisoner(Pawn pawn, Pawn talkee, PrisonerInteractionModeDef mode)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				pawn.pather.StartPath((Thing)talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition((Func<bool>)delegate()
			{
				if (talkee.Destroyed)
				{
					return true;
				}
				if (mode != PrisonerInteractionModeDefOf.Execution && !talkee.Awake())
				{
					return true;
				}
				if (!talkee.IsPrisonerOfColony)
				{
					return true;
				}
				if (talkee.guest != null && talkee.guest.interactionMode == mode)
				{
					return false;
				}
				return true;
			});
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		public static Toil WaitToBeAbleToInteract(Pawn pawn)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				if (!pawn.interactions.InteractedTooRecentlyToInteract())
				{
					pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			toil.tickAction = (Action)delegate()
			{
				if (!pawn.interactions.InteractedTooRecentlyToInteract())
				{
					pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		public static Toil ConvinceRecruitee(Pawn pawn, Pawn talkee)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				if (!pawn.interactions.TryInteractWith(talkee, InteractionDefOf.BuildRapport))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
				}
				else
				{
					pawn.records.Increment(RecordDefOf.PrisonersChatted);
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			return toil;
		}

		public static Toil SetLastInteractTime(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(targetInd).Thing;
				pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		public static Toil TryRecruit(TargetIndex recruiteeInd)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(recruiteeInd).Thing;
				if (pawn.Spawned && pawn.Awake())
				{
					InteractionDef intDef = (!pawn.def.race.Animal) ? InteractionDefOf.RecruitAttempt : InteractionDefOf.TameAttempt;
					actor.interactions.TryInteractWith(pawn, intDef);
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			return toil;
		}

		public static Toil TryTrain(TargetIndex traineeInd)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(traineeInd).Thing;
				if (pawn.Spawned && pawn.Awake() && actor.interactions.TryInteractWith(pawn, InteractionDefOf.TrainAttempt))
				{
					float statValue = actor.GetStatValue(StatDefOf.TrainAnimalChance, true);
					statValue *= Mathf.Max(0.05f, GenMath.LerpDouble(0f, 1f, 2f, 0f, pawn.RaceProps.wildness));
					if (actor.relations.DirectRelationExists(PawnRelationDefOf.Bond, pawn))
					{
						statValue = (float)(statValue * 1.5);
					}
					statValue = Mathf.Clamp01(statValue);
					TrainableDef trainableDef = pawn.training.NextTrainableToTrain();
					string text;
					if (Rand.Value < statValue)
					{
						pawn.training.Train(trainableDef, actor);
						if (pawn.caller != null)
						{
							pawn.caller.DoCall();
						}
						text = "TextMote_TrainSuccess".Translate(trainableDef.LabelCap, statValue.ToStringPercent());
						RelationsUtility.TryDevelopBondRelation(actor, pawn, 0.007f);
						TaleRecorder.RecordTale(TaleDefOf.TrainedAnimal, actor, pawn, trainableDef);
					}
					else
					{
						text = "TextMote_TrainFail".Translate(trainableDef.LabelCap, statValue.ToStringPercent());
					}
					string text2 = text;
					text = text2 + "\n" + pawn.training.GetSteps(trainableDef) + " / " + trainableDef.steps;
					MoteMaker.ThrowText((actor.DrawPos + pawn.DrawPos) / 2f, actor.Map, text, 5f);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 100;
			return toil;
		}
	}
}
