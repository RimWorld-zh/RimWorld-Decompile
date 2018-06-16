using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C78 RID: 3192
	public class PlaceWorker_ShowFacilitiesConnections : PlaceWorker
	{
		// Token: 0x060045E9 RID: 17897 RVA: 0x0024CECC File Offset: 0x0024B2CC
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
