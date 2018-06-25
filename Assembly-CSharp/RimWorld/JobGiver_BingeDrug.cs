using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000AD RID: 173
	public class JobGiver_BingeDrug : JobGiver_Binge
	{
		// Token: 0x0400027E RID: 638
		private const int BaseIngestInterval = 600;

		// Token: 0x0400027F RID: 639
		private const float OverdoseSeverityToAvoid = 0.786f;

		// Token: 0x04000280 RID: 640
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

		// Token: 0x04000281 RID: 641
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

		// Token: 0x0600042E RID: 1070 RVA: 0x00031C7C File Offset: 0x0003007C
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

		// Token: 0x0600042F RID: 1071 RVA: 0x00031D1C File Offset: 0x0003011C
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			ChemicalDef chemical = this.GetChemical(pawn);
			DrugCategory drugCategory = this.GetDrugCategory(pawn);
			Thing result;
			if (chemical == null)
			{
				Log.ErrorOnce("Tried to binge on null chemical.", 1393746152, false);
				result = null;
			}
			else
			{
				Hediff overdose = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
				Predicate<Thing> predicate = delegate(Thing t)
				{
					bool result2;
					if (!this.IgnoreForbid(pawn) && t.IsForbidden(pawn))
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
						result2 = (compDrug.Props.chemical == chemical && (overdose == null || !compDrug.Props.CanCauseOverdose || overdose.Severity + compDrug.Props.overdoseSeverityOffset.max < 0.786f) && (pawn.Position.InHorDistOf(t.Position, 60f) || t.Position.Roofed(t.Map) || pawn.Map.areaManager.Home[t.Position] || t.GetSlotGroup() != null) && t.def.ingestible.drugCategory.IncludedIn(drugCategory));
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

		// Token: 0x06000430 RID: 1072 RVA: 0x00031E10 File Offset: 0x00030210
		private ChemicalDef GetChemical(Pawn pawn)
		{
			return ((MentalState_BingingDrug)pawn.MentalState).chemical;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00031E38 File Offset: 0x00030238
		private DrugCategory GetDrugCategory(Pawn pawn)
		{
			return ((MentalState_BingingDrug)pawn.MentalState).drugCategory;
		}
	}
}
