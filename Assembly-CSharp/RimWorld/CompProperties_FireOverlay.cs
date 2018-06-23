using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000246 RID: 582
	public class CompProperties_FireOverlay : CompProperties
	{
		// Token: 0x0400048D RID: 1165
		public float fireSize = 1f;

		// Token: 0x0400048E RID: 1166
		public Vector3 offset;

		// Token: 0x06000A79 RID: 2681 RVA: 0x0005F267 File Offset: 0x0005D667
		public CompProperties_FireOverlay()
		{
			this.compClass = typeof(CompFireOverlay);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0005F28C File Offset: 0x0005D68C
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude)
		{
			Graphic graphic = GhostUtility.GhostGraphicFor(CompFireOverlay.FireGraphic, thingDef, ghostCol);
			graphic.DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef, 0f);
		}
	}
}
