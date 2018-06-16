using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200031D RID: 797
	public class IncidentWorker_Ambush_ManhunterPack : IncidentWorker_Ambush
	{
		// Token: 0x06000D92 RID: 3474 RVA: 0x00074138 File Offset: 0x00072538
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			PawnKindDef pawnKindDef;
			return base.CanFireNowSub(parms) && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, -1, out pawnKindDef);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00074170 File Offset: 0x00072570
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

		// Token: 0x06000D94 RID: 3476 RVA: 0x000741F4 File Offset: 0x000725F4
		protected override void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
			for (int i = 0; i < generatedPawns.Count; i++)
			{
				generatedPawns[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0007423C File Offset: 0x0007263C
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan == null) ? "yourCaravan".Translate() : caravan.Name, anyPawn.GetKindLabelPlural(-1)).CapitalizeFirst();
		}
	}
}
