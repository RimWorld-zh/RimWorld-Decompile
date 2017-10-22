using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class IncidentWorker_Ambush_EnemyFaction : IncidentWorker_Ambush
	{
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			List<Pawn> result;
			if (!PawnGroupMakerUtility.TryGetRandomFactionForNormalPawnGroup(parms.points, out parms.faction, (Predicate<Faction>)null, false, false, false, true))
			{
				result = new List<Pawn>();
			}
			else
			{
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(parms, false);
				defaultPawnGroupMakerParms.generateFightersOnly = true;
				result = PawnGroupMakerUtility.GeneratePawns(PawnGroupKindDefOf.Normal, defaultPawnGroupMakerParms, true).ToList();
			}
			return result;
		}

		protected override LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return new LordJob_AssaultColony(parms.faction, true, false, false, false, true);
		}

		protected override void SendAmbushLetter(Pawn anyPawn, IncidentParms parms)
		{
			base.SendStandardLetter((Thing)anyPawn, parms.faction.def.pawnsPlural, parms.faction.Name);
		}
	}
}
