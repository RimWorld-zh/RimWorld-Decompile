using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000331 RID: 817
	[HasDebugOutput]
	public class IncidentWorker_ManhunterPack : IncidentWorker
	{
		// Token: 0x06000DF8 RID: 3576 RVA: 0x000771DC File Offset: 0x000755DC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				PawnKindDef pawnKindDef;
				IntVec3 intVec;
				result = (ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, out pawnKindDef) && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, null));
			}
			return result;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0007723C File Offset: 0x0007563C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef pawnKindDef;
			bool result;
			IntVec3 intVec;
			if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, out pawnKindDef))
			{
				result = false;
			}
			else if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, null))
			{
				result = false;
			}
			else
			{
				List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals(pawnKindDef, map.Tile, parms.points * 1.4f);
				Rot4 rot = Rot4.FromAngleFlat((map.Center - intVec).AngleFlat);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i];
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
					GenSpawn.Spawn(pawn, loc, map, rot, WipeMode.Vanish, false);
					pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
					pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 135000);
				}
				Find.LetterStack.ReceiveLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate(new object[]
				{
					pawnKindDef.GetLabelPlural(-1)
				}), LetterDefOf.ThreatBig, list[0], null, null);
				Find.TickManager.slower.SignalForceNormalSpeedShort();
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
				result = true;
			}
			return result;
		}

		// Token: 0x040008D4 RID: 2260
		private const float PointsFactor = 1.4f;

		// Token: 0x040008D5 RID: 2261
		private const int AnimalsStayDurationMin = 60000;

		// Token: 0x040008D6 RID: 2262
		private const int AnimalsStayDurationMax = 135000;
	}
}
