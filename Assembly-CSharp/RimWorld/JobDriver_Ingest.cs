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
	public class JobDriver_Ingest : JobDriver
	{
		private bool usingNutrientPasteDispenser;

		private bool eatingFromInventory;

		public const float EatCorpseBodyPartsUntilFoodLevelPct = 0.9f;

		public const TargetIndex IngestibleSourceInd = TargetIndex.A;

		private const TargetIndex TableCellInd = TargetIndex.B;

		private const TargetIndex ExtraIngestiblesToCollectInd = TargetIndex.C;

		public JobDriver_Ingest()
		{
		}

		private Thing IngestibleSource
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private float ChewDurationMultiplier
		{
			get
			{
				Thing ingestibleSource = this.IngestibleSource;
				float result;
				if (ingestibleSource.def.ingestible != null && !ingestibleSource.def.ingestible.useEatingSpeedStat)
				{
					result = 1f;
				}
				else
				{
					result = 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true);
				}
				return result;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		public override string GetReport()
		{
			string result;
			if (this.usingNutrientPasteDispenser)
			{
				result = this.job.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label);
			}
			else
			{
				Thing thing = this.job.targetA.Thing;
				if (thing != null && thing.def.ingestible != null)
				{
					if (!thing.def.ingestible.ingestReportStringEat.NullOrEmpty() && (thing.def.ingestible.ingestReportString.NullOrEmpty() || this.pawn.RaceProps.intelligence < Intelligence.ToolUser))
					{
						return string.Format(thing.def.ingestible.ingestReportStringEat, this.job.targetA.Thing.LabelShort);
					}
					if (!thing.def.ingestible.ingestReportString.NullOrEmpty())
					{
						return string.Format(thing.def.ingestible.ingestReportString, this.job.targetA.Thing.LabelShort);
					}
				}
				result = base.GetReport();
			}
			return result;
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (this.IngestibleSource is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(this.IngestibleSource));
		}

		public override bool TryMakePreToilReservations()
		{
			if (this.pawn.Faction != null && !(this.IngestibleSource is Building_NutrientPasteDispenser))
			{
				Thing ingestibleSource = this.IngestibleSource;
				int num = FoodUtility.WillIngestStackCountOf(this.pawn, ingestibleSource.def, ingestibleSource.GetStatValue(StatDefOf.Nutrition, true));
				if (num >= ingestibleSource.stackCount && ingestibleSource.Spawned)
				{
					if (!this.pawn.Reserve(ingestibleSource, this.job, 1, -1, null))
					{
						return false;
					}
				}
			}
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!this.usingNutrientPasteDispenser)
			{
				this.FailOn(() => !this.IngestibleSource.Destroyed && !this.IngestibleSource.IngestibleNow);
			}
			Toil chew = Toils_Ingest.ChewIngestible(this.pawn, this.ChewDurationMultiplier, TargetIndex.A, TargetIndex.B).FailOn((Toil x) => !this.IngestibleSource.Spawned && (this.pawn.carryTracker == null || this.pawn.carryTracker.CarriedThing != this.IngestibleSource)).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			foreach (Toil toil in this.PrepareToIngestToils(chew))
			{
				yield return toil;
			}
			yield return chew;
			yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(chew, () => this.job.GetTarget(TargetIndex.A).Thing is Corpse && this.pawn.needs.food.CurLevelPercentage < 0.9f);
			yield break;
		}

		private IEnumerable<Toil> PrepareToIngestToils(Toil chewToil)
		{
			IEnumerable<Toil> result;
			if (this.usingNutrientPasteDispenser)
			{
				result = this.PrepareToIngestToils_Dispenser();
			}
			else if (this.pawn.RaceProps.ToolUser)
			{
				result = this.PrepareToIngestToils_ToolUser(chewToil);
			}
			else
			{
				result = this.PrepareToIngestToils_NonToolUser();
			}
			return result;
		}

		private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			yield return Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
			yield break;
		}

		private IEnumerable<Toil> PrepareToIngestToils_ToolUser(Toil chewToil)
		{
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else
			{
				yield return this.ReserveFoodIfWillIngestWholeStack();
				Toil gotoToPickup = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.JumpIf(gotoToPickup, () => this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.Jump(chewToil);
				yield return gotoToPickup;
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.pawn);
				Toil reserveExtraFoodToCollect = Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
				Toil findExtraFoodToCollect = new Toil();
				findExtraFoodToCollect.initAction = delegate()
				{
					if (this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.IngestibleSource.def) < this.job.takeExtraIngestibles)
					{
						Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForDef(this.IngestibleSource.def), PathEndMode.Touch, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), 12f, (Thing x) => this.pawn.CanReserve(x, 1, -1, null, false) && !x.IsForbidden(this.pawn) && x.IsSociallyProper(this.pawn), null, 0, -1, false, RegionType.Set_Passable, false);
						if (thing != null)
						{
							this.job.SetTarget(TargetIndex.C, thing);
							this.JumpToToil(reserveExtraFoodToCollect);
						}
					}
				};
				findExtraFoodToCollect.defaultCompleteMode = ToilCompleteMode.Instant;
				yield return Toils_Jump.Jump(findExtraFoodToCollect);
				yield return reserveExtraFoodToCollect;
				yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch);
				yield return Toils_Haul.TakeToInventory(TargetIndex.C, () => this.job.takeExtraIngestibles - this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.IngestibleSource.def));
				yield return findExtraFoodToCollect;
			}
			yield return Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
			yield break;
		}

		private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
		{
			yield return this.ReserveFoodIfWillIngestWholeStack();
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield break;
		}

		private Toil ReserveFoodIfWillIngestWholeStack()
		{
			return new Toil
			{
				initAction = delegate()
				{
					if (this.pawn.Faction != null)
					{
						Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
						if (this.pawn.carryTracker.CarriedThing != thing)
						{
							int num = FoodUtility.WillIngestStackCountOf(this.pawn, thing.def, thing.GetStatValue(StatDefOf.Nutrition, true));
							if (num >= thing.stackCount)
							{
								if (!thing.Spawned)
								{
									this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
								}
								else
								{
									this.pawn.Reserve(thing, this.job, 1, -1, null);
								}
							}
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant,
				atomicWithPrevious = true
			};
		}

		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 cell = this.job.GetTarget(TargetIndex.B).Cell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, cell, this.pawn);
		}

		public static bool ModifyCarriedThingDrawPosWorker(ref Vector3 drawPos, ref bool behind, ref bool flip, IntVec3 placeCell, Pawn pawn)
		{
			bool result;
			if (pawn.pather.Moving)
			{
				result = false;
			}
			else
			{
				Thing carriedThing = pawn.carryTracker.CarriedThing;
				if (carriedThing == null || !carriedThing.IngestibleNow)
				{
					result = false;
				}
				else if (placeCell.IsValid && placeCell.AdjacentToCardinal(pawn.Position) && placeCell.HasEatSurface(pawn.Map) && carriedThing.def.ingestible.ingestHoldUsesTable)
				{
					drawPos = new Vector3((float)placeCell.x + 0.5f, drawPos.y, (float)placeCell.z + 0.5f);
					result = true;
				}
				else
				{
					if (carriedThing.def.ingestible.ingestHoldOffsetStanding != null)
					{
						HoldOffset holdOffset = carriedThing.def.ingestible.ingestHoldOffsetStanding.Pick(pawn.Rotation);
						if (holdOffset != null)
						{
							drawPos += holdOffset.offset;
							behind = holdOffset.behind;
							flip = holdOffset.flip;
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		[CompilerGenerated]
		private void <ReserveFoodIfWillIngestWholeStack>m__0()
		{
			if (this.pawn.Faction != null)
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (this.pawn.carryTracker.CarriedThing != thing)
				{
					int num = FoodUtility.WillIngestStackCountOf(this.pawn, thing.def, thing.GetStatValue(StatDefOf.Nutrition, true));
					if (num >= thing.stackCount)
					{
						if (!thing.Spawned)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
						}
						else
						{
							this.pawn.Reserve(thing, this.job, 1, -1, null);
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <chew>__0;

			internal IEnumerator<Toil> $locvar0;

			internal Toil <toil>__1;

			internal JobDriver_Ingest $this;

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
					if (!this.usingNutrientPasteDispenser)
					{
						this.FailOn(() => !base.IngestibleSource.Destroyed && !base.IngestibleSource.IngestibleNow);
					}
					chew = Toils_Ingest.ChewIngestible(this.pawn, base.ChewDurationMultiplier, TargetIndex.A, TargetIndex.B).FailOn((Toil x) => !base.IngestibleSource.Spawned && (this.pawn.carryTracker == null || this.pawn.carryTracker.CarriedThing != base.IngestibleSource)).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					enumerator = base.PrepareToIngestToils(chew).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					this.$current = Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Jump.JumpIf(chew, () => this.job.GetTarget(TargetIndex.A).Thing is Corpse && this.pawn.needs.food.CurLevelPercentage < 0.9f);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
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
						toil = enumerator.Current;
						this.$current = toil;
						if (!this.$disposing)
						{
							this.$PC = 1;
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
				this.$current = chew;
				if (!this.$disposing)
				{
					this.$PC = 2;
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
				case 1u:
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
				JobDriver_Ingest.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Ingest.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !base.IngestibleSource.Destroyed && !base.IngestibleSource.IngestibleNow;
			}

			internal bool <>m__1(Toil x)
			{
				return !base.IngestibleSource.Spawned && (this.pawn.carryTracker == null || this.pawn.carryTracker.CarriedThing != base.IngestibleSource);
			}

			internal bool <>m__2()
			{
				return this.job.GetTarget(TargetIndex.A).Thing is Corpse && this.pawn.needs.food.CurLevelPercentage < 0.9f;
			}
		}

		[CompilerGenerated]
		private sealed class <PrepareToIngestToils_Dispenser>c__Iterator1 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_Ingest $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PrepareToIngestToils_Dispenser>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedNullOrForbidden(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
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
				JobDriver_Ingest.<PrepareToIngestToils_Dispenser>c__Iterator1 <PrepareToIngestToils_Dispenser>c__Iterator = new JobDriver_Ingest.<PrepareToIngestToils_Dispenser>c__Iterator1();
				<PrepareToIngestToils_Dispenser>c__Iterator.$this = this;
				return <PrepareToIngestToils_Dispenser>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <PrepareToIngestToils_ToolUser>c__Iterator2 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <gotoToPickup>__1;

			internal Toil chewToil;

			internal Toil <findExtraFoodToCollect>__2;

			internal JobDriver_Ingest $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_Ingest.<PrepareToIngestToils_ToolUser>c__Iterator2.<PrepareToIngestToils_ToolUser>c__AnonStorey4 $locvar0;

			[DebuggerHidden]
			public <PrepareToIngestToils_ToolUser>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.eatingFromInventory)
					{
						this.$current = Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					this.$current = base.ReserveFoodIfWillIngestWholeStack();
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 1u:
					break;
				case 2u:
					gotoToPickup = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.$current = Toils_Jump.JumpIf(gotoToPickup, () => this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Jump.Jump(chewToil);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = gotoToPickup;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Ingest.PickupIngestible(TargetIndex.A, this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
				{
					Toil reserveExtraFoodToCollect = Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
					findExtraFoodToCollect = new Toil();
					findExtraFoodToCollect.initAction = delegate()
					{
						if (this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.IngestibleSource.def) < this.job.takeExtraIngestibles)
						{
							Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForDef(this.IngestibleSource.def), PathEndMode.Touch, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), 12f, (Thing x) => this.pawn.CanReserve(x, 1, -1, null, false) && !x.IsForbidden(this.pawn) && x.IsSociallyProper(this.pawn), null, 0, -1, false, RegionType.Set_Passable, false);
							if (thing != null)
							{
								this.job.SetTarget(TargetIndex.C, thing);
								this.JumpToToil(reserveExtraFoodToCollect);
							}
						}
					};
					findExtraFoodToCollect.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = Toils_Jump.Jump(findExtraFoodToCollect);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				}
				case 8u:
					this.$current = <PrepareToIngestToils_ToolUser>c__AnonStorey.reserveExtraFoodToCollect;
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				case 10u:
					this.$current = Toils_Haul.TakeToInventory(TargetIndex.C, () => <PrepareToIngestToils_ToolUser>c__AnonStorey.<>f__ref$2.$this.job.takeExtraIngestibles - <PrepareToIngestToils_ToolUser>c__AnonStorey.<>f__ref$2.$this.pawn.inventory.innerContainer.TotalStackCountOfDef(<PrepareToIngestToils_ToolUser>c__AnonStorey.<>f__ref$2.$this.IngestibleSource.def));
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				case 11u:
					this.$current = findExtraFoodToCollect;
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				case 12u:
					break;
				case 13u:
					this.$current = Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 14;
					}
					return true;
				case 14u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				this.$current = Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedOrNull(TargetIndex.A);
				if (!this.$disposing)
				{
					this.$PC = 13;
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
				JobDriver_Ingest.<PrepareToIngestToils_ToolUser>c__Iterator2 <PrepareToIngestToils_ToolUser>c__Iterator = new JobDriver_Ingest.<PrepareToIngestToils_ToolUser>c__Iterator2();
				<PrepareToIngestToils_ToolUser>c__Iterator.$this = this;
				<PrepareToIngestToils_ToolUser>c__Iterator.chewToil = chewToil;
				return <PrepareToIngestToils_ToolUser>c__Iterator;
			}

			internal bool <>m__0()
			{
				return this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			}

			private sealed class <PrepareToIngestToils_ToolUser>c__AnonStorey4
			{
				internal Toil reserveExtraFoodToCollect;

				internal JobDriver_Ingest.<PrepareToIngestToils_ToolUser>c__Iterator2 <>f__ref$2;

				public <PrepareToIngestToils_ToolUser>c__AnonStorey4()
				{
				}

				internal void <>m__0()
				{
					if (this.<>f__ref$2.$this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.<>f__ref$2.$this.IngestibleSource.def) < this.<>f__ref$2.$this.job.takeExtraIngestibles)
					{
						Thing thing = GenClosest.ClosestThingReachable(this.<>f__ref$2.$this.pawn.Position, this.<>f__ref$2.$this.pawn.Map, ThingRequest.ForDef(this.<>f__ref$2.$this.IngestibleSource.def), PathEndMode.Touch, TraverseParms.For(this.<>f__ref$2.$this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), 12f, (Thing x) => this.<>f__ref$2.$this.pawn.CanReserve(x, 1, -1, null, false) && !x.IsForbidden(this.<>f__ref$2.$this.pawn) && x.IsSociallyProper(this.<>f__ref$2.$this.pawn), null, 0, -1, false, RegionType.Set_Passable, false);
						if (thing != null)
						{
							this.<>f__ref$2.$this.job.SetTarget(TargetIndex.C, thing);
							this.<>f__ref$2.$this.JumpToToil(this.reserveExtraFoodToCollect);
						}
					}
				}

				internal int <>m__1()
				{
					return this.<>f__ref$2.$this.job.takeExtraIngestibles - this.<>f__ref$2.$this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.<>f__ref$2.$this.IngestibleSource.def);
				}

				internal bool <>m__2(Thing x)
				{
					return this.<>f__ref$2.$this.pawn.CanReserve(x, 1, -1, null, false) && !x.IsForbidden(this.<>f__ref$2.$this.pawn) && x.IsSociallyProper(this.<>f__ref$2.$this.pawn);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PrepareToIngestToils_NonToolUser>c__Iterator3 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_Ingest $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PrepareToIngestToils_NonToolUser>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = base.ReserveFoodIfWillIngestWholeStack();
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
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
				JobDriver_Ingest.<PrepareToIngestToils_NonToolUser>c__Iterator3 <PrepareToIngestToils_NonToolUser>c__Iterator = new JobDriver_Ingest.<PrepareToIngestToils_NonToolUser>c__Iterator3();
				<PrepareToIngestToils_NonToolUser>c__Iterator.$this = this;
				return <PrepareToIngestToils_NonToolUser>c__Iterator;
			}
		}
	}
}
