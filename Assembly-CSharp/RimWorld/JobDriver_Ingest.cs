using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000097 RID: 151
	public class JobDriver_Ingest : JobDriver
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0002B3B0 File Offset: 0x000297B0
		private Thing IngestibleSource
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0002B3DC File Offset: 0x000297DC
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

		// Token: 0x060003CE RID: 974 RVA: 0x0002B43F File Offset: 0x0002983F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0002B46C File Offset: 0x0002986C
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

		// Token: 0x060003D0 RID: 976 RVA: 0x0002B5AC File Offset: 0x000299AC
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (this.IngestibleSource is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(this.IngestibleSource));
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0002B604 File Offset: 0x00029A04
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

		// Token: 0x060003D2 RID: 978 RVA: 0x0002B6A0 File Offset: 0x00029AA0
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

		// Token: 0x060003D3 RID: 979 RVA: 0x0002B6CC File Offset: 0x00029ACC
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

		// Token: 0x060003D4 RID: 980 RVA: 0x0002B724 File Offset: 0x00029B24
		private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			yield return Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
			yield break;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0002B750 File Offset: 0x00029B50
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

		// Token: 0x060003D6 RID: 982 RVA: 0x0002B784 File Offset: 0x00029B84
		private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
		{
			yield return this.ReserveFoodIfWillIngestWholeStack();
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield break;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0002B7B0 File Offset: 0x00029BB0
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

		// Token: 0x060003D8 RID: 984 RVA: 0x0002B7EC File Offset: 0x00029BEC
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 cell = this.job.GetTarget(TargetIndex.B).Cell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, cell, this.pawn);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0002B828 File Offset: 0x00029C28
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

		// Token: 0x0400025B RID: 603
		private bool usingNutrientPasteDispenser;

		// Token: 0x0400025C RID: 604
		private bool eatingFromInventory;

		// Token: 0x0400025D RID: 605
		public const float EatCorpseBodyPartsUntilFoodLevelPct = 0.9f;

		// Token: 0x0400025E RID: 606
		public const TargetIndex IngestibleSourceInd = TargetIndex.A;

		// Token: 0x0400025F RID: 607
		private const TargetIndex TableCellInd = TargetIndex.B;

		// Token: 0x04000260 RID: 608
		private const TargetIndex ExtraIngestiblesToCollectInd = TargetIndex.C;
	}
}
