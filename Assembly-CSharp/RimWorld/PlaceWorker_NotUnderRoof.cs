using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C66 RID: 3174
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		// Token: 0x060045CF RID: 17871 RVA: 0x0024D958 File Offset: 0x0024BD58
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			AcceptanceReport result;
			if (map.roofGrid.Roofed(loc))
			{
				result = new AcceptanceReport("MustPlaceUnroofed".Translate());
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
