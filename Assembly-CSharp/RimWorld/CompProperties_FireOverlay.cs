using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000246 RID: 582
	public class CompProperties_FireOverlay : CompProperties
	{
		// Token: 0x06000A7B RID: 2683 RVA: 0x0005F20B File Offset: 0x0005D60B
		public CompProperties_FireOverlay()
		{
			this.compClass = typeof(CompFireOverlay);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0005F230 File Offset: 0x0005D630
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude)
		{
			Graphic graphic = GhostUtility.GhostGraphicFor(CompFireOverlay.FireGraphic, thingDef, ghostCol);
			graphic.DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef, 0f);
		}

		// Token: 0x0400048F RID: 1167
		public float fireSize = 1f;

		// Token: 0x04000490 RID: 1168
		public Vector3 offset;
	}
}
