using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000032 RID: 50
	public abstract class JobDriver_InteractAnimal : JobDriver
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00013538 File Offset: 0x00011938
		protected Pawn Animal
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00013562 File Offset: 0x00011962
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.feedNutritionLeft, "feedNutritionLeft", 0f, false);
		}

		// Token: 0x060001C8 RID: 456
		protected abstract Toil FinalInteractToil();

		// Token: 0x060001C9 RID: 457 RVA: 0x00013584 File Offset: 0x00011984
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Animal, this.job, 1, -1, null);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000135B8 File Offset: 0x000119B8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return JobDriver_InteractAnimal.TalkToAnimal(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return JobDriver_InteractAnimal.TalkToAnimal(TargetIndex.A);
			foreach (Toil t in this.FeedToils())
			{
				yield return t;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return JobDriver_InteractAnimal.TalkToAnimal(TargetIndex.A);
			foreach (Toil t2 in this.FeedToils())
			{
				yield return t2;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.FinalInteractToil();
			yield break;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000135E4 File Offset: 0x000119E4
		public static float RequiredNutritionPerFeed(Pawn animal)
		{
			return Mathf.Min(animal.needs.food.MaxLevel * 0.15f, 0.3f);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0001361C File Offset: 0x00011A1C
		private IEnumerable<Toil> FeedToils()
		{
			yield return new Toil
			{
				initAction = delegate()
				{
					this.feedNutritionLeft = JobDriver_InteractAnimal.RequiredNutritionPerFeed(this.Animal);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil gotoAnimal = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return gotoAnimal;
			yield return this.StartFeedAnimal(TargetIndex.A);
			yield return Toils_Ingest.FinalizeIngest(this.Animal, TargetIndex.B);
			yield return Toils_General.PutCarriedThingInInventory();
			yield return Toils_General.ClearTarget(TargetIndex.B);
			yield return Toils_Jump.JumpIf(gotoAnimal, () => this.feedNutritionLeft > 0f);
			yield break;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00013648 File Offset: 0x00011A48
		private static Toil TalkToAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn recipient = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.AnimalChat);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 270;
			return toil;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000136B0 File Offset: 0x00011AB0
		private Toil StartFeedAnimal(TargetIndex tameeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				PawnUtility.ForceWait(pawn, 270, actor, false);
				Thing thing = FoodUtility.BestFoodInInventory(actor, pawn, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty, 0f, false);
				if (thing == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
					int num = FoodUtility.StackCountForNutrition(this.feedNutritionLeft, thing.GetStatValue(StatDefOf.Nutrition, true));
					int stackCount = thing.stackCount;
					Thing thing2 = actor.inventory.innerContainer.Take(thing, Mathf.Min(num, stackCount));
					actor.carryTracker.TryStartCarry(thing2);
					actor.CurJob.SetTarget(TargetIndex.B, thing2);
					float num2 = (float)thing2.stackCount * thing2.GetStatValue(StatDefOf.Nutrition, true);
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
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			return toil;
		}

		// Token: 0x040001B8 RID: 440
		protected const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x040001B9 RID: 441
		private const TargetIndex FoodHandInd = TargetIndex.B;

		// Token: 0x040001BA RID: 442
		private const int FeedDuration = 270;

		// Token: 0x040001BB RID: 443
		private const int TalkDuration = 270;

		// Token: 0x040001BC RID: 444
		private const float NutritionPercentagePerFeed = 0.15f;

		// Token: 0x040001BD RID: 445
		private const float MaxMinNutritionPerFeed = 0.3f;

		// Token: 0x040001BE RID: 446
		public const int FeedCount = 2;

		// Token: 0x040001BF RID: 447
		public const FoodPreferability MaxFoodPreferability = FoodPreferability.RawTasty;

		// Token: 0x040001C0 RID: 448
		private float feedNutritionLeft;
	}
}
