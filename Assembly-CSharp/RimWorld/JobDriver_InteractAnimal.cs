using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
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

		protected JobDriver_InteractAnimal()
		{
		}

		protected Pawn Animal
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
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
			return this.pawn.Reserve(this.Animal, this.job, 1, -1, null);
		}

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

		public static float RequiredNutritionPerFeed(Pawn animal)
		{
			return Mathf.Min(animal.needs.food.MaxLevel * 0.15f, 0.3f);
		}

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

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal IEnumerator<Toil> $locvar0;

			internal Toil <t>__1;

			internal IEnumerator<Toil> $locvar1;

			internal Toil <t>__2;

			internal JobDriver_InteractAnimal $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.FailOnDowned(TargetIndex.A);
					this.FailOnNotCasualInterruptible(TargetIndex.A);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = JobDriver_InteractAnimal.TalkToAnimal(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = JobDriver_InteractAnimal.TalkToAnimal(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					enumerator = base.FeedToils().GetEnumerator();
					num = 4294967293u;
					break;
				case 9u:
					break;
				case 10u:
					this.$current = Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				case 11u:
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				case 12u:
					this.$current = JobDriver_InteractAnimal.TalkToAnimal(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				case 13u:
					enumerator2 = base.FeedToils().GetEnumerator();
					num = 4294967293u;
					goto Block_15;
				case 14u:
					goto IL_2E4;
				case 15u:
					this.$current = Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 16;
					}
					return true;
				case 16u:
					this.$current = Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 17;
					}
					return true;
				case 17u:
					this.$current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 18;
					}
					return true;
				case 18u:
					this.$current = this.FinalInteractToil();
					if (!this.$disposing)
					{
						this.$PC = 19;
					}
					return true;
				case 19u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						t = enumerator.Current;
						this.$current = t;
						if (!this.$disposing)
						{
							this.$PC = 9;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				if (!this.$disposing)
				{
					this.$PC = 10;
				}
				return true;
				Block_15:
				try
				{
					IL_2E4:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						t2 = enumerator2.Current;
						this.$current = t2;
						if (!this.$disposing)
						{
							this.$PC = 14;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				if (!this.$disposing)
				{
					this.$PC = 15;
				}
				return true;
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 9u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 14u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
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
				JobDriver_InteractAnimal.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_InteractAnimal.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <FeedToils>c__Iterator1 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <init>__0;

			internal Toil <gotoAnimal>__0;

			internal JobDriver_InteractAnimal $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <FeedToils>c__Iterator1()
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
					Toil init = new Toil();
					init.initAction = delegate()
					{
						this.feedNutritionLeft = JobDriver_InteractAnimal.RequiredNutritionPerFeed(base.Animal);
					};
					init.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = init;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
					gotoAnimal = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					this.$current = gotoAnimal;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = base.StartFeedAnimal(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Ingest.FinalizeIngest(base.Animal, TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_General.PutCarriedThingInInventory();
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_General.ClearTarget(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Jump.JumpIf(gotoAnimal, () => this.feedNutritionLeft > 0f);
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
				JobDriver_InteractAnimal.<FeedToils>c__Iterator1 <FeedToils>c__Iterator = new JobDriver_InteractAnimal.<FeedToils>c__Iterator1();
				<FeedToils>c__Iterator.$this = this;
				return <FeedToils>c__Iterator;
			}

			internal void <>m__0()
			{
				this.feedNutritionLeft = JobDriver_InteractAnimal.RequiredNutritionPerFeed(base.Animal);
			}

			internal bool <>m__1()
			{
				return this.feedNutritionLeft > 0f;
			}
		}

		[CompilerGenerated]
		private sealed class <TalkToAnimal>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex tameeInd;

			public <TalkToAnimal>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.GetActor();
				Pawn recipient = (Pawn)((Thing)actor.CurJob.GetTarget(this.tameeInd));
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.AnimalChat);
			}
		}

		[CompilerGenerated]
		private sealed class <StartFeedAnimal>c__AnonStorey3
		{
			internal Toil toil;

			internal TargetIndex tameeInd;

			internal JobDriver_InteractAnimal $this;

			public <StartFeedAnimal>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.GetActor();
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(this.tameeInd));
				PawnUtility.ForceWait(pawn, 270, actor, false);
				Thing thing = FoodUtility.BestFoodInInventory(actor, pawn, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty, 0f, false);
				if (thing == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
					int num = FoodUtility.StackCountForNutrition(this.$this.feedNutritionLeft, thing.GetStatValue(StatDefOf.Nutrition, true));
					int stackCount = thing.stackCount;
					Thing thing2 = actor.inventory.innerContainer.Take(thing, Mathf.Min(num, stackCount));
					actor.carryTracker.TryStartCarry(thing2);
					actor.CurJob.SetTarget(TargetIndex.B, thing2);
					float num2 = (float)thing2.stackCount * thing2.GetStatValue(StatDefOf.Nutrition, true);
					this.$this.ticksLeftThisToil = Mathf.CeilToInt(270f * (num2 / JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn)));
					if (num <= stackCount)
					{
						this.$this.feedNutritionLeft = 0f;
					}
					else
					{
						this.$this.feedNutritionLeft -= num2;
						if (this.$this.feedNutritionLeft < 0.001f)
						{
							this.$this.feedNutritionLeft = 0f;
						}
					}
				}
			}
		}
	}
}
