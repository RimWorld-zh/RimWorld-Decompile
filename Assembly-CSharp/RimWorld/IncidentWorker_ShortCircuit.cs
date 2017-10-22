using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			return ShortCircuitUtility.GetShortCircuitablePowerConduits(map).Any();
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IEnumerable<Building> shortCircuitablePowerConduits = ShortCircuitUtility.GetShortCircuitablePowerConduits(map);
			Building culprit = default(Building);
			bool result;
			if (!shortCircuitablePowerConduits.TryRandomElement<Building>(out culprit))
			{
				result = false;
			}
			else
			{
				ShortCircuitUtility.DoShortCircuit(culprit);
				result = true;
			}
			return result;
		}
	}
}
