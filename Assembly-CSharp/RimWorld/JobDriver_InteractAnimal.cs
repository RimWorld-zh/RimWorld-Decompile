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
				return (Pawn)base.job.targetA.Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.feedNutritionLeft, "feedNutritionLeft", 0f, false);
		}

		protected abstract Toil FinalInteractToil();

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.Animal, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public static float RequiredNutritionPerFeed(Pawn animal)
		{
			return Mathf.Min((float)(animal.needs.food.MaxLevel * 0.15000000596046448), 0.3f);
		}

		private IEnumerable<Toil> FeedToils()
		{
			yield return new Toil
			{
				initAction = delegate
				{
					((_003CFeedToils_003Ec__Iterator1)/*Error near IL_004a: stateMachine*/)._0024this.feedNutritionLeft = JobDriver_InteractAnimal.RequiredNutritionPerFeed(((_003CFeedToils_003Ec__Iterator1)/*Error near IL_004a: stateMachine*/)._0024this.Animal);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private static Toil TalkToAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
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
			toil.initAction = delegate
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
