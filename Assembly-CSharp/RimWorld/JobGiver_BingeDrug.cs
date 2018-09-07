using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_BingeDrug : JobGiver_Binge
	{
		private const int BaseIngestInterval = 600;

		private const float OverdoseSeverityToAvoid = 0.786f;

		private static readonly SimpleCurve IngestIntervalFactorCurve_Drunkness = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 4f),
				true
			}
		};

		private static readonly SimpleCurve IngestIntervalFactorCurve_DrugOverdose = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 5f),
				true
			}
		};

		public JobGiver_BingeDrug()
		{
		}

		protected override int IngestInterval(Pawn pawn)
		{
			ChemicalDef chemical = this.GetChemical(pawn);
			int num = 600;
			if (chemical == ChemicalDefOf.Alcohol)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh, false);
				if (firstHediffOfDef != null)
				{
					num = (int)((float)num * JobGiver_BingeDrug.IngestIntervalFactorCurve_Drunkness.Evaluate(firstHediffOfDef.Severity));
				}
			}
			else
			{
				Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
				if (firstHediffOfDef2 != null)
				{
					num = (int)((float)num * JobGiver_BingeDrug.IngestIntervalFactorCurve_DrugOverdose.Evaluate(firstHediffOfDef2.Severity));
				}
			}
			return num;
		}

		protected override Thing BestIngestTarget(Pawn pawn)
		{
			ChemicalDef chemical = this.GetChemical(pawn);
			DrugCategory drugCategory = this.GetDrugCategory(pawn);
			if (chemical == null)
			{
				Log.ErrorOnce("Tried to binge on null chemical.", 1393746152, false);
				return null;
			}
			Hediff overdose = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
			Predicate<Thing> predicate = delegate(Thing t)
			{
				if (!this.IgnoreForbid(pawn) && t.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				CompDrug compDrug = t.TryGetComp<CompDrug>();
				return compDrug.Props.chemical == chemical && (overdose == null || !compDrug.Props.CanCauseOverdose || overdose.Severity + compDrug.Props.overdoseSeverityOffset.max < 0.786f) && (pawn.Position.InHorDistOf(t.Position, 60f) || t.Position.Roofed(t.Map) || pawn.Map.areaManager.Home[t.Position] || t.GetSlotGroup() != null) && t.def.ingestible.drugCategory.IncludedIn(drugCategory);
			};
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			ThingRequest thingReq = ThingRequest.ForGroup(ThingRequestGroup.Drug);
			PathEndMode peMode = PathEndMode.OnCell;
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			return GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		private ChemicalDef GetChemical(Pawn pawn)
		{
			return ((MentalState_BingingDrug)pawn.MentalState).chemical;
		}

		private DrugCategory GetDrugCategory(Pawn pawn)
		{
			return ((MentalState_BingingDrug)pawn.MentalState).drugCategory;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JobGiver_BingeDrug()
		{
		}

		[CompilerGenerated]
		private sealed class <BestIngestTarget>c__AnonStorey0
		{
			internal Pawn pawn;

			internal ChemicalDef chemical;

			internal Hediff overdose;

			internal DrugCategory drugCategory;

			internal JobGiver_BingeDrug $this;

			public <BestIngestTarget>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				if (!this.$this.IgnoreForbid(this.pawn) && t.IsForbidden(this.pawn))
				{
					return false;
				}
				if (!this.pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				CompDrug compDrug = t.TryGetComp<CompDrug>();
				return compDrug.Props.chemical == this.chemical && (this.overdose == null || !compDrug.Props.CanCauseOverdose || this.overdose.Severity + compDrug.Props.overdoseSeverityOffset.max < 0.786f) && (this.pawn.Position.InHorDistOf(t.Position, 60f) || t.Position.Roofed(t.Map) || this.pawn.Map.areaManager.Home[t.Position] || t.GetSlotGroup() != null) && t.def.ingestible.drugCategory.IncludedIn(this.drugCategory);
			}
		}
	}
}
