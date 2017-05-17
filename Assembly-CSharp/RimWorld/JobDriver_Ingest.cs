using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				return 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true);
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
			Thing thing = this.pawn.CurJob.targetA.Thing;
			if (thing != null && thing.def.ingestible != null && !thing.def.ingestible.ingestReportString.NullOrEmpty())
			{
				return string.Format(thing.def.ingestible.ingestReportString, this.pawn.CurJob.targetA.Thing.LabelShort);
			}
			return base.GetReport();
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (this.IngestibleSource is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(this.IngestibleSource));
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Ingest.<MakeNewToils>c__Iterator4D <MakeNewToils>c__Iterator4D = new JobDriver_Ingest.<MakeNewToils>c__Iterator4D();
			<MakeNewToils>c__Iterator4D.<>f__this = this;
			JobDriver_Ingest.<MakeNewToils>c__Iterator4D expr_0E = <MakeNewToils>c__Iterator4D;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private IEnumerable<Toil> PrepareToIngestToils(Toil chewToil)
		{
			if (this.usingNutrientPasteDispenser)
			{
				return this.PrepareToIngestToils_Dispenser();
			}
			if (this.pawn.RaceProps.ToolUser)
			{
				return this.PrepareToIngestToils_ToolUser(chewToil);
			}
			return this.PrepareToIngestToils_NonToolUser();
		}

		[DebuggerHidden]
		private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
		{
			JobDriver_Ingest.<PrepareToIngestToils_Dispenser>c__Iterator4E <PrepareToIngestToils_Dispenser>c__Iterator4E = new JobDriver_Ingest.<PrepareToIngestToils_Dispenser>c__Iterator4E();
			<PrepareToIngestToils_Dispenser>c__Iterator4E.<>f__this = this;
			JobDriver_Ingest.<PrepareToIngestToils_Dispenser>c__Iterator4E expr_0E = <PrepareToIngestToils_Dispenser>c__Iterator4E;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[DebuggerHidden]
		private IEnumerable<Toil> PrepareToIngestToils_ToolUser(Toil chewToil)
		{
			JobDriver_Ingest.<PrepareToIngestToils_ToolUser>c__Iterator4F <PrepareToIngestToils_ToolUser>c__Iterator4F = new JobDriver_Ingest.<PrepareToIngestToils_ToolUser>c__Iterator4F();
			<PrepareToIngestToils_ToolUser>c__Iterator4F.chewToil = chewToil;
			<PrepareToIngestToils_ToolUser>c__Iterator4F.<$>chewToil = chewToil;
			<PrepareToIngestToils_ToolUser>c__Iterator4F.<>f__this = this;
			JobDriver_Ingest.<PrepareToIngestToils_ToolUser>c__Iterator4F expr_1C = <PrepareToIngestToils_ToolUser>c__Iterator4F;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		[DebuggerHidden]
		private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
		{
			JobDriver_Ingest.<PrepareToIngestToils_NonToolUser>c__Iterator50 <PrepareToIngestToils_NonToolUser>c__Iterator = new JobDriver_Ingest.<PrepareToIngestToils_NonToolUser>c__Iterator50();
			<PrepareToIngestToils_NonToolUser>c__Iterator.<>f__this = this;
			JobDriver_Ingest.<PrepareToIngestToils_NonToolUser>c__Iterator50 expr_0E = <PrepareToIngestToils_NonToolUser>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private Toil ReserveFoodIfWillIngestWholeStack()
		{
			return new Toil
			{
				initAction = delegate
				{
					if (this.pawn.Faction == null)
					{
						return;
					}
					Thing thing = this.pawn.CurJob.GetTarget(TargetIndex.A).Thing;
					if (this.pawn.carryTracker.CarriedThing == thing)
					{
						return;
					}
					int num = FoodUtility.WillIngestStackCountOf(this.pawn, thing.def);
					if (num >= thing.stackCount)
					{
						if (!thing.Spawned)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
							return;
						}
						this.pawn.Reserve(thing, 1, -1, null);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 cell = base.CurJob.GetTarget(TargetIndex.B).Cell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, cell, this.pawn);
		}

		public static bool ModifyCarriedThingDrawPosWorker(ref Vector3 drawPos, ref bool behind, ref bool flip, IntVec3 placeCell, Pawn pawn)
		{
			if (pawn.pather.Moving)
			{
				return false;
			}
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing == null || !carriedThing.IngestibleNow)
			{
				return false;
			}
			if (placeCell.IsValid && placeCell.AdjacentToCardinal(pawn.Position) && placeCell.HasEatSurface(pawn.Map) && carriedThing.def.ingestible.ingestHoldUsesTable)
			{
				drawPos = new Vector3((float)placeCell.x + 0.5f, drawPos.y, (float)placeCell.z + 0.5f);
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
	}
}
