using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C72 RID: 3186
	public class PlaceWorker_DeepDrill : PlaceWorker_ShowDeepResources
	{
		// Token: 0x060045D9 RID: 17881 RVA: 0x0024C988 File Offset: 0x0024AD88
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
