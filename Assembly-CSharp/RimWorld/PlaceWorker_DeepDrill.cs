using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C70 RID: 3184
	public class PlaceWorker_DeepDrill : PlaceWorker_ShowDeepResources
	{
		// Token: 0x060045E3 RID: 17891 RVA: 0x0024DE0C File Offset: 0x0024C20C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			AcceptanceReport result;
			if (DeepDrillUtility.GetNextResource(loc, map) == null)
			{
				result = "MustPlaceOnDrillable".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
