using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6D RID: 3181
	public class PlaceWorker_HeadOnShipBeam : PlaceWorker
	{
		// Token: 0x060045CF RID: 17871 RVA: 0x0024C760 File Offset: 0x0024AB60
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			IntVec3 c = loc + rot.FacingCell * -1;
			AcceptanceReport result;
			if (!c.InBounds(map))
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(map);
				if (edifice == null || edifice.def != ThingDefOf.Ship_Beam)
				{
					result = "MustPlaceHeadOnShipBeam".Translate();
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
	}
}
