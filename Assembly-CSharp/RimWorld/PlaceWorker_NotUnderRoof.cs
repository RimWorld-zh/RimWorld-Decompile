using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6A RID: 3178
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		// Token: 0x060045C8 RID: 17864 RVA: 0x0024C5B0 File Offset: 0x0024A9B0
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
