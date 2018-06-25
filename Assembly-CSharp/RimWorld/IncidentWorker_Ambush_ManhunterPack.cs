using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200031F RID: 799
	public class IncidentWorker_Ambush_ManhunterPack : IncidentWorker_Ambush
	{
		// Token: 0x06000D95 RID: 3477 RVA: 0x00074344 File Offset: 0x00072744
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			PawnKindDef pawnKindDef;
			return base.CanFireNowSub(parms) && ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, -1, out pawnKindDef);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0007437C File Offset: 0x0007277C
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

		// Token: 0x06000D97 RID: 3479 RVA: 0x00074400 File Offset: 0x00072800
		protected override void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
			for (int i = 0; i < generatedPawns.Count; i++)
			{
				generatedPawns[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00074448 File Offset: 0x00072848
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan == null) ? "yourCaravan".Translate() : caravan.Name, anyPawn.GetKindLabelPlural(-1)).CapitalizeFirst();
		}
	}
}
