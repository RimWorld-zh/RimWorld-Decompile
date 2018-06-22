using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000341 RID: 833
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		// Token: 0x06000E3D RID: 3645 RVA: 0x00078FA8 File Offset: 0x000773A8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return ShortCircuitUtility.GetShortCircuitablePowerConduits(map).Any<Building>();
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00078FD4 File Offset: 0x000773D4
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
