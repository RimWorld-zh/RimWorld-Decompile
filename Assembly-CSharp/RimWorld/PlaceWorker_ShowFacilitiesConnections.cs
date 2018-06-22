using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C74 RID: 3188
	public class PlaceWorker_ShowFacilitiesConnections : PlaceWorker
	{
		// Token: 0x060045F0 RID: 17904 RVA: 0x0024E274 File Offset: 0x0024C674
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			if (def.HasComp(typeof(CompAffectedByFacilities)))
			{
				CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
			}
			else
			{
				CompFacility.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
			}
		}
	}
}
