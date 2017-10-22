using System;
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
			Thing result;
			if (chemical == null)
			{
				Log.ErrorOnce("Tried to binge on null chemical.", 1393746152);
				result = null;
			}
			else
			{
				Hediff overdose = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
				Predicate<Thing> predicate = (Predicate<Thing>)delegate(Thing t)
				{
					bool result2;
					if (!base.IgnoreForbid(pawn) && t.IsForbidden(pawn))
					{
						result2 = false;
					}
					else if (!pawn.CanReserve(t, 1, -1, null, false))
					{
						result2 = false;
					}
					else
					{
						CompDrug compDrug = t.TryGetComp<CompDrug>();
						result2 = ((byte)((compDrug.Props.chemical == chemical) ? ((overdose == null || !compDrug.Props.CanCauseOverdose || !(overdose.Severity + compDrug.Props.overdoseSeverityOffset.max >= 0.78600001335144043)) ? ((pawn.Position.InHorDistOf(t.Position, 60f) || t.Position.Roofed(t.Map) || ((Area)pawn.Map.areaManager.Home)[t.Position] || t.GetSlotGroup() != null) ? 1 : 0) : 0) : 0) != 0);
					}
					return result2;
				};
				IntVec3 position = pawn.Position;
				Map map = pawn.Map;
				ThingRequest thingReq = ThingRequest.ForGroup(ThingRequestGroup.Drug);
				PathEndMode peMode = PathEndMode.OnCell;
				TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
				Predicate<Thing> validator = predicate;
				result = GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			}
			return result;
		}

		private ChemicalDef GetChemical(Pawn pawn)
		{
			return ((MentalState_BingingDrug)pawn.MentalState).chemical;
		}
	}
}
