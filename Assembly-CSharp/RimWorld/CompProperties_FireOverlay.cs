using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000248 RID: 584
	public class CompProperties_FireOverlay : CompProperties
	{
		// Token: 0x0400048D RID: 1165
		public float fireSize = 1f;

		// Token: 0x0400048E RID: 1166
		public Vector3 offset;

		// Token: 0x06000A7D RID: 2685 RVA: 0x0005F3B7 File Offset: 0x0005D7B7
		public CompProperties_FireOverlay()
		{
			this.compClass = typeof(CompFireOverlay);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0005F3DC File Offset: 0x0005D7DC
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude)
		{
			Graphic graphic = GhostUtility.GhostGraphicFor(CompFireOverlay.FireGraphic, thingDef, ghostCol);
			graphic.DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef, 0f);
		}
	}
}
