using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6E RID: 3182
	public class PlaceWorker_DeepDrill : PlaceWorker_ShowDeepResources
	{
		// Token: 0x060045E0 RID: 17888 RVA: 0x0024DD30 File Offset: 0x0024C130
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
