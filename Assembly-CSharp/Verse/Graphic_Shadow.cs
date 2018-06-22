using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDE RID: 3550
	public class Graphic_Shadow : Graphic
	{
		// Token: 0x06004F8F RID: 20367 RVA: 0x002969F4 File Offset: 0x00294DF4
		public Graphic_Shadow(ShadowData shadowInfo)
		{
			this.shadowInfo = shadowInfo;
			if (shadowInfo == null)
			{
				throw new ArgumentNullException("shadowInfo");
			}
			this.shadowMesh = ShadowMeshPool.GetShadowMesh(shadowInfo);
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x00296A24 File Offset: 0x00294E24
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (this.shadowMesh != null)
			{
				if (thingDef != null && this.shadowInfo != null && (Find.CurrentMap == null || !loc.ToIntVec3().InBounds(Find.CurrentMap) || !Find.CurrentMap.roofGrid.Roofed(loc.ToIntVec3())) && DebugViewSettings.drawShadows)
				{
					Vector3 position = loc + this.shadowInfo.offset;
					position.y = AltitudeLayer.Shadows.AltitudeFor();
					Graphics.DrawMesh(this.shadowMesh, position, rot.AsQuat, MatBases.SunShadowFade, 0);
				}
			}
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x00296AD4 File Offset: 0x00294ED4
		public override void Print(SectionLayer layer, Thing thing)
		{
			Vector3 center = thing.TrueCenter() + (this.shadowInfo.offset + new Vector3(Graphic_Shadow.GlobalShadowPosOffsetX, 0f, Graphic_Shadow.GlobalShadowPosOffsetZ)).RotatedBy(thing.Rotation);
			center.y = AltitudeLayer.Shadows.AltitudeFor();
			Printer_Shadow.PrintShadow(layer, center, this.shadowInfo, thing.Rotation);
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x00296B40 File Offset: 0x00294F40
		public override string ToString()
		{
			return "Graphic_Shadow(" + this.shadowInfo + ")";
		}

		// Token: 0x040034C7 RID: 13511
		private Mesh shadowMesh;

		// Token: 0x040034C8 RID: 13512
		private ShadowData shadowInfo;

		// Token: 0x040034C9 RID: 13513
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetX = 0f;

		// Token: 0x040034CA RID: 13514
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetZ = 0f;
	}
}
