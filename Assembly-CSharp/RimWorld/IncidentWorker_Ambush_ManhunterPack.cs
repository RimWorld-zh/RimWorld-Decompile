using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200031D RID: 797
	public class IncidentWorker_Ambush_ManhunterPack : IncidentWorker_Ambush
	{
		// Token: 0x06000D92 RID: 3474 RVA: 0x000741EC File Offset: 0x000725EC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			PawnKindDef pawnKindDef;
			return base.CanFireNowSub(parms) && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, -1, out pawnKindDef);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00074224 File Offset: 0x00072624
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			PawnKindDef animalKind;
			List<Pawn> result;
			if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, parms.target.Tile, out animalKind) && !ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, -1, out animalKind))
			{
				Log.Error("Could not find any valid animal kind for " + this.def + " incident.", false);
				result = new List<Pawn>();
			}
			else
			{
				result = ManhunterPackIncidentUtility.GenerateAnimals(animalKind, parms.target.Tile, parms.points);
			}
			return result;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x000742A8 File Offset: 0x000726A8
		protected override void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
			for (int i = 0; i < generatedPawns.Count; i++)
			{
				generatedPawns[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x000742F0 File Offset: 0x000726F0
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan == null) ? "yourCaravan".Translate() : caravan.Name, anyPawn.GetKindLabelPlural(-1)).CapitalizeFirst();
		}
	}
}
