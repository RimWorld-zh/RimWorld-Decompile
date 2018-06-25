using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		public IncidentWorker_ShortCircuit()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return ShortCircuitUtility.GetShortCircuitablePowerConduits(map).Any<Building>();
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IEnumerable<Building> shortCircuitablePowerConduits = ShortCircuitUtility.GetShortCircuitablePowerConduits(map);
			Building culprit;
			bool result;
			if (!shortCircuitablePowerConduits.TryRandomElement(out culprit))
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
