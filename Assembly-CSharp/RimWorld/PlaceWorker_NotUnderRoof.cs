using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C69 RID: 3177
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		// Token: 0x060045C6 RID: 17862 RVA: 0x0024C588 File Offset: 0x0024A988
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
