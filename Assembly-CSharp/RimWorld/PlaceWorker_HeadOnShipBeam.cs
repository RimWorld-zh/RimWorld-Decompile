using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C69 RID: 3177
	public class PlaceWorker_HeadOnShipBeam : PlaceWorker
	{
		// Token: 0x060045D6 RID: 17878 RVA: 0x0024DB08 File Offset: 0x0024BF08
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
