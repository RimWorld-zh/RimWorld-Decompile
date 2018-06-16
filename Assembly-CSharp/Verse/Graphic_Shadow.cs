using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE2 RID: 3554
	public class Graphic_Shadow : Graphic
	{
		// Token: 0x06004F7C RID: 20348 RVA: 0x00295438 File Offset: 0x00293838
		public Graphic_Shadow(ShadowData shadowInfo)
		{
			this.shadowInfo = shadowInfo;
			if (shadowInfo == null)
			{
				throw new ArgumentNullException("shadowInfo");
			}
			this.shadowMesh = ShadowMeshPool.GetShadowMesh(shadowInfo);
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x00295468 File Offset: 0x00293868
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

		// Token: 0x06004F7E RID: 20350 RVA: 0x00295518 File Offset: 0x00293918
		public override void Print(SectionLayer layer, Thing thing)
		{
			Vector3 center = thing.TrueCenter() + (this.shadowInfo.offset + new Vector3(Graphic_Shadow.GlobalShadowPosOffsetX, 0f, Graphic_Shadow.GlobalShadowPosOffsetZ)).RotatedBy(thing.Rotation);
			center.y = AltitudeLayer.Shadows.AltitudeFor();
			Printer_Shadow.PrintShadow(layer, center, this.shadowInfo, thing.Rotation);
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x00295584 File Offset: 0x00293984
		public override string ToString()
		{
			return "Graphic_Shadow(" + this.shadowInfo + ")";
		}

		// Token: 0x040034BE RID: 13502
		private Mesh shadowMesh;

		// Token: 0x040034BF RID: 13503
		private ShadowData shadowInfo;

		// Token: 0x040034C0 RID: 13504
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetX = 0f;

		// Token: 0x040034C1 RID: 13505
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetZ = 0f;
	}
}
