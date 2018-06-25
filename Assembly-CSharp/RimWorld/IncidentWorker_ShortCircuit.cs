using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000343 RID: 835
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		// Token: 0x06000E41 RID: 3649 RVA: 0x000790F8 File Offset: 0x000774F8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return ShortCircuitUtility.GetShortCircuitablePowerConduits(map).Any<Building>();
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00079124 File Offset: 0x00077524
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
