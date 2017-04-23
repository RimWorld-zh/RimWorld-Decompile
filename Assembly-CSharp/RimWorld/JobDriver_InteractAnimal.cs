using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_InteractAnimal.<MakeNewToils>c__Iterator5 <MakeNewToils>c__Iterator = new JobDriver_InteractAnimal.<MakeNewToils>c__Iterator5();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_InteractAnimal.<MakeNewToils>c__Iterator5 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public static float RequiredNutritionPerFeed(Pawn animal)
		{
			return Mathf.Min(animal.needs.food.MaxLevel * 0.15f, 0.3f);
		}

		[DebuggerHidden]
		private IEnumerable<Toil> FeedToils()
		{
			JobDriver_InteractAnimal.<FeedToils>c__Iterator6 <FeedToils>c__Iterator = new JobDriver_InteractAnimal.<FeedToils>c__Iterator6();
			<FeedToils>c__Iterator.<>f__this = this;
			JobDriver_InteractAnimal.<FeedToils>c__Iterator6 expr_0E = <FeedToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private static Toil TalkToAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.GetActor();
				Pawn recipient = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
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
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				PawnUtility.ForceWait(pawn, 270, actor, false);
				Thing thing = FoodUtility.BestFoodInInventory(actor, pawn, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty, 0f, false);
				if (thing == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					return;
				}
				actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
				int num = FoodUtility.StackCountForNutrition(thing.def, this.feedNutritionLeft);
				int stackCount = thing.stackCount;
				Thing thing2 = actor.inventory.innerContainer.Take(thing, Mathf.Min(num, stackCount));
				actor.carryTracker.TryStartCarry(thing2);
				actor.CurJob.SetTarget(TargetIndex.B, thing2);
				float num2 = (float)thing2.stackCount * thing2.def.ingestible.nutrition;
				this.ticksLeftThisToil = Mathf.CeilToInt(270f * (num2 / JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn)));
				if (num <= stackCount)
				{
					this.feedNutritionLeft = 0f;
				}
				else
				{
					this.feedNutritionLeft -= num2;
					if (this.feedNutritionLeft < 0.001f)
					{
						this.feedNutritionLeft = 0f;
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			return toil;
		}
	}
}
