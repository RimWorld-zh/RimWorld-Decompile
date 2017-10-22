using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class RaidStrategyWorker_ImmediateAttack : RaidStrategyWorker
	{
		public override LordJob MakeLordJob(IncidentParms parms, Map map)
		{
			LordJob result;
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				result = new LordJob_AssaultColony(parms.faction, true, true, false, false, true);
			}
			else
			{
				IntVec3 fallbackLocation = default(IntVec3);
				RCellFinder.TryFindRandomSpotJustOutsideColony(parms.spawnCenter, map, out fallbackLocation);
				result = new LordJob_AssistColony(parms.faction, fallbackLocation);
			}
			return result;
		}
	}
}
