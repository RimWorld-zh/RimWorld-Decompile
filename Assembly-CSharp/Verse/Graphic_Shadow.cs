using System;
using UnityEngine;

namespace Verse
{
	public class Graphic_Shadow : Graphic
	{
		private Mesh shadowMesh;

		private ShadowData shadowInfo;

		public Graphic_Shadow(ShadowData shadowInfo)
		{
			this.shadowInfo = shadowInfo;
			if (shadowInfo == null)
			{
				throw new ArgumentNullException("shadowInfo");
			}
			this.shadowMesh = ShadowMeshPool.GetShadowMesh(shadowInfo);
		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing)
		{
			if ((UnityEngine.Object)this.shadowMesh != (UnityEngine.Object)null && thingDef != null && this.shadowInfo != null)
			{
				if (Find.VisibleMap != null && loc.ToIntVec3().InBounds(Find.VisibleMap) && Find.VisibleMap.roofGrid.Roofed(loc.ToIntVec3()))
					return;
				Vector3 position = loc + this.shadowInfo.offset;
				position.y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
				Graphics.DrawMesh(this.shadowMesh, position, rot.AsQuat, MatBases.SunShadowFade, 0);
			}
		}

		public override void Print(SectionLayer layer, Thing thing)
		{
			Vector3 center = thing.TrueCenter() + this.shadowInfo.offset.RotatedBy(thing.Rotation);
			center.y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
			Printer_Shadow.PrintShadow(layer, center, this.shadowInfo, thing.Rotation);
		}

		public override string ToString()
		{
			return "Graphic_Shadow(" + this.shadowInfo + ")";
		}
	}
}
