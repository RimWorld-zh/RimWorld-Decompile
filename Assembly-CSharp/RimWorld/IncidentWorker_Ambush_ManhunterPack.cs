using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_Ambush_ManhunterPack : IncidentWorker_Ambush
	{
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			PawnKindDef pawnKindDef = default(PawnKindDef);
			return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, -1, out pawnKindDef) && base.TryExecuteWorker(parms);
		}

		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			PawnKindDef animalKind = default(PawnKindDef);
			List<Pawn> result;
			if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, parms.target.Tile, out animalKind) && !ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, -1, out animalKind))
			{
				Log.Error("Could not find any valid animal kind for " + base.def + " incident.");
				result = new List<Pawn>();
			}
			else
			{
				result = ManhunterPackIncidentUtility.GenerateAnimals(animalKind, parms.target.Tile, parms.points);
			}
			return result;
		}

		protected override void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
			for (int i = 0; i < generatedPawns.Count; i++)
			{
				generatedPawns[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, (string)null, false, false, null);
			}
		}

		protected override void SendAmbushLetter(Pawn anyPawn, IncidentParms parms)
		{
			base.SendStandardLetter((Thing)anyPawn, anyPawn.GetKindLabelPlural(-1));
		}
	}
}
