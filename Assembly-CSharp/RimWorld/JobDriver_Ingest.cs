using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Ingest : JobDriver
	{
		public const float EatCorpseBodyPartsUntilFoodLevelPct = 0.9f;

		public const TargetIndex IngestibleSourceInd = TargetIndex.A;

		private const TargetIndex TableCellInd = TargetIndex.B;

		private const TargetIndex ExtraIngestiblesToCollectInd = TargetIndex.C;

		private bool usingNutrientPasteDispenser;

		private bool eatingFromInventory;

		private Thing IngestibleSource
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private float ChewDurationMultiplier
		{
			get
			{
				Thing ingestibleSource = this.IngestibleSource;
				if (ingestibleSource.def.ingestible != null && !ingestibleSource.def.ingestible.useEatingSpeedStat)
				{
					return 1f;
				}
				return (float)(1.0 / base.pawn.GetStatValue(StatDefOf.EatingSpeed, true));
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
			if (this.usingNutrientPasteDispenser)
			{
				return base.CurJob.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label);
			}
			Thing thing = base.pawn.CurJob.targetA.Thing;
			if (thing != null && thing.def.ingestible != null && !thing.def.ingestible.ingestReportString.NullOrEmpty())
			{
				return string.Format(thing.def.ingestible.ingestReportString, base.pawn.CurJob.targetA.Thing.LabelShort);
			}
			return base.GetReport();
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (this.IngestibleSource is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (base.pawn.inventory != null && base.pawn.inventory.Contains(this.IngestibleSource));
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!this.usingNutrientPasteDispenser)
			{
				this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator4D)/*Error near IL_0045: stateMachine*/)._003C_003Ef__this.IngestibleSource.Destroyed && !((_003CMakeNewToils_003Ec__Iterator4D)/*Error near IL_0045: stateMachine*/)._003C_003Ef__this.IngestibleSource.IngestibleNow));
			}
			Toil chew = Toils_Ingest.ChewIngestible(base.pawn, this.ChewDurationMultiplier, TargetIndex.A, TargetIndex.B).FailOn((Func<Toil, bool>)((Toil x) => !((_003CMakeNewToils_003Ec__Iterator4D)/*Error near IL_0075: stateMachine*/)._003C_003Ef__this.IngestibleSource.Spawned && (((_003CMakeNewToils_003Ec__Iterator4D)/*Error near IL_0075: stateMachine*/)._003C_003Ef__this.pawn.carryTracker == null || ((_003CMakeNewToils_003Ec__Iterator4D)/*Error near IL_0075: stateMachine*/)._003C_003Ef__this.pawn.carryTracker.CarriedThing != ((_003CMakeNewToils_003Ec__Iterator4D)/*Error near IL_0075: stateMachine*/)._003C_003Ef__this.IngestibleSource))).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			foreach (Toil item in this.PrepareToIngestToils(chew))
			{
				yield return item;
			}
			yield return chew;
			yield return Toils_Ingest.FinalizeIngest(base.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(chew, (Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator4D)/*Error near IL_015d: stateMachine*/)._003C_003Ef__this.CurJob.GetTarget(TargetIndex.A).Thing is Corpse && ((_003CMakeNewToils_003Ec__Iterator4D)/*Error near IL_015d: stateMachine*/)._003C_003Ef__this.pawn.needs.food.CurLevelPercentage < 0.89999997615814209));
		}

		private IEnumerable<Toil> PrepareToIngestToils(Toil chewToil)
		{
			if (this.usingNutrientPasteDispenser)
			{
				return this.PrepareToIngestToils_Dispenser();
			}
			if (base.pawn.RaceProps.ToolUser)
			{
				return this.PrepareToIngestToils_ToolUser(chewToil);
			}
			return this.PrepareToIngestToils_NonToolUser();
		}

		private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, base.pawn);
			yield return Toils_Ingest.CarryIngestibleToChewSpot(base.pawn, TargetIndex.A).FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
		}

		private IEnumerable<Toil> PrepareToIngestToils_ToolUser(Toil chewToil)
		{
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(base.pawn, TargetIndex.A);
			}
			else
			{
				yield return this.ReserveFoodIfWillIngestWholeStack();
				Toil gotoToPickup = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.JumpIf(gotoToPickup, (Func<bool>)(() => ((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_00c4: stateMachine*/)._003C_003Ef__this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)));
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.Jump(chewToil);
				yield return gotoToPickup;
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, base.pawn);
				Toil reserveExtraFoodToCollect = Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
				Toil findExtraFoodToCollect = new Toil
				{
					initAction = (Action)delegate()
					{
						if (((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.pawn.inventory.innerContainer.TotalStackCountOfDef(((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.IngestibleSource.def) < ((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.CurJob.takeExtraIngestibles)
						{
							Predicate<Thing> validator = (Predicate<Thing>)((Thing x) => ((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.pawn.CanReserve(x, 1, -1, null, false) && !x.IsForbidden(((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.pawn) && x.IsSociallyProper(((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.pawn));
							Thing thing = GenClosest.ClosestThingReachable(((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.pawn.Position, ((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.pawn.Map, ThingRequest.ForDef(((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.IngestibleSource.def), PathEndMode.Touch, TraverseParms.For(((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), 12f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
							if (thing != null)
							{
								((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.pawn.CurJob.SetTarget(TargetIndex.C, thing);
								((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003C_003Ef__this.JumpToToil(((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_017d: stateMachine*/)._003CreserveExtraFoodToCollect_003E__1);
							}
						}
					},
					defaultCompleteMode = ToilCompleteMode.Instant
				};
				yield return Toils_Jump.Jump(findExtraFoodToCollect);
				yield return reserveExtraFoodToCollect;
				yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch);
				yield return Toils_Haul.TakeToInventory(TargetIndex.C, (Func<int>)(() => ((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_01ec: stateMachine*/)._003C_003Ef__this.CurJob.takeExtraIngestibles - ((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_01ec: stateMachine*/)._003C_003Ef__this.pawn.inventory.innerContainer.TotalStackCountOfDef(((_003CPrepareToIngestToils_ToolUser_003Ec__Iterator4F)/*Error near IL_01ec: stateMachine*/)._003C_003Ef__this.IngestibleSource.def)));
				yield return findExtraFoodToCollect;
			}
			yield return Toils_Ingest.CarryIngestibleToChewSpot(base.pawn, TargetIndex.A).FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
		}

		private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
		{
			yield return this.ReserveFoodIfWillIngestWholeStack();
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
		}

		private Toil ReserveFoodIfWillIngestWholeStack()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
			{
				if (base.pawn.Faction != null)
				{
					Thing thing = base.pawn.CurJob.GetTarget(TargetIndex.A).Thing;
					if (base.pawn.carryTracker.CarriedThing != thing)
					{
						int num = FoodUtility.WillIngestStackCountOf(base.pawn, thing.def);
						if (num >= thing.stackCount)
						{
							if (!thing.Spawned)
							{
								base.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
							}
							else
							{
								base.pawn.Reserve(thing, 1, -1, null);
							}
						}
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 cell = base.CurJob.GetTarget(TargetIndex.B).Cell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, cell, base.pawn);
		}

		public static bool ModifyCarriedThingDrawPosWorker(ref Vector3 drawPos, ref bool behind, ref bool flip, IntVec3 placeCell, Pawn pawn)
		{
			if (pawn.pather.Moving)
			{
				return false;
			}
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing != null && carriedThing.IngestibleNow)
			{
				if (placeCell.IsValid && placeCell.AdjacentToCardinal(pawn.Position) && placeCell.HasEatSurface(pawn.Map) && carriedThing.def.ingestible.ingestHoldUsesTable)
				{
					drawPos = new Vector3((float)((float)placeCell.x + 0.5), drawPos.y, (float)((float)placeCell.z + 0.5));
					return true;
				}
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
				return false;
			}
			return false;
		}
	}
}
