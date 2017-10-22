using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class RaidStrategyWorker_ImmediateAttackSmart : RaidStrategyWorker
	{
		public override LordJob MakeLordJob(IncidentParms parms, Map map)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, false, true, true);
		}

		public override bool CanUseWith(IncidentParms parms)
		{
			if (!base.CanUseWith(parms))
			{
				return false;
			}
			if (!parms.faction.def.canUseAvoidGrid)
			{
				return false;
			}
			return true;
		}
	}
}
