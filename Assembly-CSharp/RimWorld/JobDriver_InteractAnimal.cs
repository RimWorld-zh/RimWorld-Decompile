using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_InteractAnimal : JobDriver
	{
		protected const TargetIndex AnimalInd = TargetIndex.A;

		private const TargetIndex FoodHandInd = TargetIndex.B;

		private const int FeedDuration = 270;

		private const int TalkDuration = 270;

		private const float NutritionPercentagePerFeed = 0.15f;

		private const float MaxMinNutritionPerFeed = 0.3f;

		public const int FeedCount = 2;

		public const FoodPreferability MaxFoodPreferability = FoodPreferability.RawTasty;

		private float feedNutritionLeft;

		protected Pawn Animal
		{
			get
			{
				return (Pawn)base.CurJob.targetA.Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.feedNutritionLeft, "feedNutritionLeft", 0f, false);
		}

		protected abstract Toil FinalInteractToil();

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(base.pawn);
			yield return JobDriver_InteractAnimal.TalkToAnimal(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(base.pawn);
			yield return JobDriver_InteractAnimal.TalkToAnimal(TargetIndex.A);
			foreach (Toil item in this.FeedToils())
			{
				yield return item;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(base.pawn);
			yield return JobDriver_InteractAnimal.TalkToAnimal(TargetIndex.A);
			foreach (Toil item2 in this.FeedToils())
			{
				yield return item2;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(base.pawn);
			yield return this.FinalInteractToil();
		}

		public static float RequiredNutritionPerFeed(Pawn animal)
		{
			return Mathf.Min((float)(animal.needs.food.MaxLevel * 0.15000000596046448), 0.3f);
		}

		private IEnumerable<Toil> FeedToils()
		{
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CFeedToils_003Ec__Iterator6)/*Error near IL_004a: stateMachine*/)._003C_003Ef__this.feedNutritionLeft = JobDriver_InteractAnimal.RequiredNutritionPerFeed(((_003CFeedToils_003Ec__Iterator6)/*Error near IL_004a: stateMachine*/)._003C_003Ef__this.Animal);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil gotoAnimal = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return gotoAnimal;
			yield return this.StartFeedAnimal(TargetIndex.A);
			yield return Toils_Ingest.FinalizeIngest(this.Animal, TargetIndex.B);
			yield return Toils_General.PutCarriedThingInInventory();
			yield return Toils_General.ClearTarget(TargetIndex.B);
			yield return Toils_Jump.JumpIf(gotoAnimal, (Func<bool>)(() => ((_003CFeedToils_003Ec__Iterator6)/*Error near IL_011b: stateMachine*/)._003C_003Ef__this.feedNutritionLeft > 0.0));
		}

		private static Toil TalkToAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn recipient = (Pawn)(Thing)actor.CurJob.GetTarget(tameeInd);
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.AnimalChat);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 270;
			return toil;
		}

		private Toil StartFeedAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn pawn = (Pawn)(Thing)actor.CurJob.GetTarget(tameeInd);
				PawnUtility.ForceWait(pawn, 270, actor, false);
				Thing thing = FoodUtility.BestFoodInInventory(actor, pawn, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty, 0f, false);
				if (thing == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
					int num = FoodUtility.StackCountForNutrition(thing.def, this.feedNutritionLeft);
					int stackCount = thing.stackCount;
					Thing thing2 = actor.inventory.innerContainer.Take(thing, Mathf.Min(num, stackCount));
					actor.carryTracker.TryStartCarry(thing2);
					actor.CurJob.SetTarget(TargetIndex.B, thing2);
					float num2 = (float)thing2.stackCount * thing2.def.ingestible.nutrition;
					base.ticksLeftThisToil = Mathf.CeilToInt((float)(270.0 * (num2 / JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn))));
					if (num <= stackCount)
					{
						this.feedNutritionLeft = 0f;
					}
					else
					{
						this.feedNutritionLeft -= num2;
						if (this.feedNutritionLeft < 0.0010000000474974513)
						{
							this.feedNutritionLeft = 0f;
						}
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			return toil;
		}
	}
}
