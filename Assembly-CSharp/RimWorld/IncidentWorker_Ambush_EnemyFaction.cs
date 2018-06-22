using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200031C RID: 796
	public class IncidentWorker_Ambush_EnemyFaction : IncidentWorker_Ambush
	{
		// Token: 0x06000D8D RID: 3469 RVA: 0x00074094 File Offset: 0x00072494
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return base.CanFireNowSub(parms) && PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out faction, null, false, false, false, true);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x000740D0 File Offset: 0x000724D0
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

		// Token: 0x06000D8F RID: 3471 RVA: 0x00074150 File Offset: 0x00072550
		protected override LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return new LordJob_AssaultColony(parms.faction, true, false, false, false, true);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00074178 File Offset: 0x00072578
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan == null) ? "yourCaravan".Translate() : caravan.Name, parms.faction.def.pawnsPlural, parms.faction.Name).CapitalizeFirst();
		}
	}
}
