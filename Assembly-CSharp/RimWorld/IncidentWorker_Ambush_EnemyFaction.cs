using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200031E RID: 798
	public class IncidentWorker_Ambush_EnemyFaction : IncidentWorker_Ambush
	{
		// Token: 0x06000D91 RID: 3473 RVA: 0x000741E4 File Offset: 0x000725E4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return base.CanFireNowSub(parms) && PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out faction, null, false, false, false, true);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00074220 File Offset: 0x00072620
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			List<Pawn> result;
			if (!PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out parms.faction, null, false, false, false, true))
			{
				Log.Error("Could not find any valid faction for " + this.def + " incident.", false);
				result = new List<Pawn>();
			}
			else
			{
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, parms, false);
				defaultPawnGroupMakerParms.generateFightersOnly = true;
				defaultPawnGroupMakerParms.dontUseSingleUseRocketLaunchers = true;
				result = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
			}
			return result;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000742A0 File Offset: 0x000726A0
		protected override LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return new LordJob_AssaultColony(parms.faction, true, false, false, false, true);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x000742C8 File Offset: 0x000726C8
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan == null) ? "yourCaravan".Translate() : caravan.Name, parms.faction.def.pawnsPlural, parms.faction.Name).CapitalizeFirst();
		}
	}
}
