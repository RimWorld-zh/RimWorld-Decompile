using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000343 RID: 835
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		// Token: 0x06000E40 RID: 3648 RVA: 0x00079100 File Offset: 0x00077500
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return ShortCircuitUtility.GetShortCircuitablePowerConduits(map).Any<Building>();
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0007912C File Offset: 0x0007752C
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
