using System;
using System.Collections.Generic;
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

		private Thing IngestibleSource
		{
			get
			{
				return base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private float ChewDurationMultiplier
		{
			get
			{
				Thing ingestibleSource = this.IngestibleSource;
				return (float)((ingestibleSource.def.ingestible == null || ingestibleSource.def.ingestible.useEatingSpeedStat) ? (1.0 / base.pawn.GetStatValue(StatDefOf.EatingSpeed, true)) : 1.0);
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
				result = base.job.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label);
			}
			else
			{
				Thing thing = base.job.targetA.Thing;
				if (thing != null && thing.def.ingestible != null)
				{
					if (!thing.def.ingestible.ingestReportStringEat.NullOrEmpty() && (thing.def.ingestible.ingestReportString.NullOrEmpty() || (int)base.pawn.RaceProps.intelligence < 1))
					{
						result = string.Format(thing.def.ingestible.ingestReportStringEat, base.job.targetA.Thing.LabelShort);
						goto IL_0130;
					}
					if (!thing.def.ingestible.ingestReportString.NullOrEmpty())
					{
						result = string.Format(thing.def.ingestible.ingestReportString, base.job.targetA.Thing.LabelShort);
						goto IL_0130;
					}
				}
				result = base.GetReport();
			}
			goto IL_0130;
			IL_0130:
			return result;
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (this.IngestibleSource is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (base.pawn.inventory != null && base.pawn.inventory.Contains(this.IngestibleSource));
		}

		public override bool TryMakePreToilReservations()
		{
			bool result;
			if (base.pawn.Faction != null && !(this.IngestibleSource is Building_NutrientPasteDispenser))
			{
				Thing ingestibleSource = this.IngestibleSource;
				int num = FoodUtility.WillIngestStackCountOf(base.pawn, ingestibleSource.def);
				if (num >= ingestibleSource.stackCount && ingestibleSource.Spawned && !base.pawn.Reserve(ingestibleSource, base.job, 1, -1, null))
				{
					result = false;
					goto IL_0082;
				}
			}
			result = true;
			goto IL_0082;
			IL_0082:
			return result;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!this.usingNutrientPasteDispenser)
			{
				this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0047: stateMachine*/)._0024this.IngestibleSource.Destroyed && !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0047: stateMachine*/)._0024this.IngestibleSource.IngestibleNow));
			}
			Toil chew = Toils_Ingest.ChewIngestible(base.pawn, this.ChewDurationMultiplier, TargetIndex.A, TargetIndex.B).FailOn((Func<Toil, bool>)((Toil x) => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0078: stateMachine*/)._0024this.IngestibleSource.Spawned && (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0078: stateMachine*/)._0024this.pawn.carryTracker == null || ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0078: stateMachine*/)._0024this.pawn.carryTracker.CarriedThing != ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0078: stateMachine*/)._0024this.IngestibleSource))).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			using (IEnumerator<Toil> enumerator = this.PrepareToIngestToils(chew).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Toil toil = enumerator.Current;
					yield return toil;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield return chew;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_01b0:
			/*Error near IL_01b1: Unexpected return in MoveNext()*/;
		}

		private IEnumerable<Toil> PrepareToIngestToils(Toil chewToil)
		{
			return (!this.usingNutrientPasteDispenser) ? ((!base.pawn.RaceProps.ToolUser) ? this.PrepareToIngestToils_NonToolUser() : this.PrepareToIngestToils_ToolUser(chewToil)) : this.PrepareToIngestToils_Dispenser();
		}

		private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private IEnumerable<Toil> PrepareToIngestToils_ToolUser(Toil chewToil)
		{
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(base.pawn, TargetIndex.A);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return this.ReserveFoodIfWillIngestWholeStack();
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
		{
			yield return this.ReserveFoodIfWillIngestWholeStack();
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private Toil ReserveFoodIfWillIngestWholeStack()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
			{
				if (base.pawn.Faction != null)
				{
					Thing thing = base.job.GetTarget(TargetIndex.A).Thing;
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
								base.pawn.Reserve(thing, base.job, 1, -1, null);
							}
						}
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 cell = base.job.GetTarget(TargetIndex.B).Cell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, cell, base.pawn);
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
					drawPos = new Vector3((float)((float)placeCell.x + 0.5), drawPos.y, (float)((float)placeCell.z + 0.5));
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
							result = true;
							goto IL_0124;
						}
					}
					result = false;
				}
			}
			goto IL_0124;
			IL_0124:
			return result;
		}
	}
}
