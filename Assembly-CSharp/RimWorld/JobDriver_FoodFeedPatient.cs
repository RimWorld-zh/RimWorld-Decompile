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
	public class JobDriver_FoodFeedPatient : JobDriver
	{
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		private const TargetIndex DelivereeInd = TargetIndex.B;

		private const float FeedDurationMultiplier = 1.5f;

		public JobDriver_FoodFeedPatient()
		{
		}

		protected Thing Food
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		public override string GetReport()
		{
			string result;
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				result = this.job.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", this.Deliveree.LabelShort);
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		public override bool TryMakePreToilReservations()
		{
			bool result;
			if (!this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null))
			{
				result = false;
			}
			else
			{
				if (!(base.TargetThingA is Building_NutrientPasteDispenser) && (this.pawn.inventory == null || !this.pawn.inventory.Contains(base.TargetThingA)))
				{
					if (!this.pawn.Reserve(this.Food, this.job, 1, -1, null))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !FoodUtility.ShouldBeFedBySomeone(this.Deliveree));
			if (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA))
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else if (base.TargetThingA is Building_NutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.ChewIngestible(this.Deliveree, 1.5f, TargetIndex.A, TargetIndex.None).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.Deliveree, TargetIndex.A);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_FoodFeedPatient $this;

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
				switch (num)
				{
				case 0u:
					this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
					this.FailOn(() => !FoodUtility.ShouldBeFedBySomeone(base.Deliveree));
					if (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA))
					{
						this.$current = Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					if (base.TargetThingA is Building_NutrientPasteDispenser)
					{
						this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 1u:
					break;
				case 2u:
					this.$current = Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					break;
				case 4u:
					this.$current = Toils_Ingest.PickupIngestible(TargetIndex.A, base.Deliveree);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					break;
				case 6u:
					this.$current = Toils_Ingest.ChewIngestible(base.Deliveree, 1.5f, TargetIndex.A, TargetIndex.None).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = Toils_Ingest.FinalizeIngest(base.Deliveree, TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				this.$current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
				if (!this.$disposing)
				{
					this.$PC = 6;
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
				JobDriver_FoodFeedPatient.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_FoodFeedPatient.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !FoodUtility.ShouldBeFedBySomeone(base.Deliveree);
			}
		}
	}
}
