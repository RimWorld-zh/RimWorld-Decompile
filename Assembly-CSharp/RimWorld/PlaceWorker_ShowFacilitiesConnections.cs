using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C77 RID: 3191
	public class PlaceWorker_ShowFacilitiesConnections : PlaceWorker
	{
		// Token: 0x060045F3 RID: 17907 RVA: 0x0024E630 File Offset: 0x0024CA30
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
