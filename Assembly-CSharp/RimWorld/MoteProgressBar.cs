using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D2 RID: 1746
	[StaticConstructorOnStartup]
	public class MoteProgressBar : MoteDualAttached
	{
		// Token: 0x0400151F RID: 5407
		public float progress;

		// Token: 0x04001520 RID: 5408
		public float offsetZ;

		// Token: 0x04001521 RID: 5409
		private static readonly Material UnfilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f, 0.65f), ShaderDatabase.MetaOverlay);

		// Token: 0x04001522 RID: 5410
		private static readonly Material FilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f, 0.65f), ShaderDatabase.MetaOverlay);

		// Token: 0x060025E8 RID: 9704 RVA: 0x00144E98 File Offset: 0x00143298
		public override void Draw()
		{
			base.UpdatePositionAndRotation();
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = this.exactPosition;
				r.center.z = r.center.z + this.offsetZ;
				r.size = new Vector2(this.exactScale.x, this.exactScale.z);
				r.fillPercent = this.progress;
				r.filledMat = MoteProgressBar.FilledMat;
				r.unfilledMat = MoteProgressBar.UnfilledMat;
				r.margin = 0.12f;
				if (this.offsetZ >= -0.8f && this.offsetZ <= -0.3f && this.AnyThingWithQualityHere())
				{
					r.center.z = r.center.z + 0.25f;
				}
				GenDraw.DrawFillableBar(r);
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x00144F88 File Offset: 0x00143388
		private bool AnyThingWithQualityHere()
		{
			IntVec3 c = this.exactPosition.ToIntVec3();
			bool result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].TryGetComp<CompQuality>() != null && (thingList[i].DrawPos - this.exactPosition).MagnitudeHorizontalSquared() < 0.0001f)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
