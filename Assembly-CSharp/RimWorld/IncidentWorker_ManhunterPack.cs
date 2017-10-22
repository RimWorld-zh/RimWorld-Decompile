using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_ManhunterPack : IncidentWorker
	{
		private const float PointsFactor = 1.4f;

		private const int AnimalsStayDurationMin = 60000;

		private const int AnimalsStayDurationMax = 135000;

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef pawnKindDef = default(PawnKindDef);
			if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, out pawnKindDef))
			{
				return false;
			}
			IntVec3 intVec = default(IntVec3);
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, (Predicate<IntVec3>)null))
			{
				return false;
			}
			List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals(pawnKindDef, map.Tile, (float)(parms.points * 1.3999999761581421));
			Rot4 rot = Rot4.FromAngleFlat((map.Center - intVec).AngleFlat);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
				GenSpawn.Spawn(pawn, loc, map, rot, false);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, (string)null, false, false, null);
				pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 135000);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelManhunterPackArrived".Translate(), "ManhunterPackArrived".Translate(pawnKindDef.label), LetterDefOf.BadUrgent, (Thing)list[0], (string)null);
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
			return true;
		}
	}
}
