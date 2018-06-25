using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C68 RID: 3176
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		// Token: 0x060045D2 RID: 17874 RVA: 0x0024DA34 File Offset: 0x0024BE34
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
