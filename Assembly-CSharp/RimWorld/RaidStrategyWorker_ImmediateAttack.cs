using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A6 RID: 422
	public class RaidStrategyWorker_ImmediateAttack : RaidStrategyWorker
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x00052664 File Offset: 0x00050A64
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 originCell = (!parms.spawnCenter.IsValid) ? pawns[0].PositionHeld : parms.spawnCenter;
			LordJob result;
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				result = new LordJob_AssaultColony(parms.faction, true, true, false, false, true);
			}
			else
			{
				IntVec3 fallbackLocation;
				RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, out fallbackLocation);
				result = new LordJob_AssistColony(parms.faction, fallbackLocation);
			}
			return result;
		}
	}
}
