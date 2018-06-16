using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB4 RID: 3252
	public abstract class SkyOverlay
	{
		// Token: 0x0600479C RID: 18332 RVA: 0x000A51CB File Offset: 0x000A35CB
		public SkyOverlay()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.OverlayColor = Color.clear;
			});
		}

		// Token: 0x17000B4C RID: 2892
		// (set) Token: 0x0600479D RID: 18333 RVA: 0x000A51F3 File Offset: 0x000A35F3
		public Color OverlayColor
		{
			set
			{
				if (this.worldOverlayMat != null)
				{
					this.worldOverlayMat.color = value;
				}
				if (this.screenOverlayMat != null)
				{
					this.screenOverlayMat.color = value;
				}
			}
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x000A5230 File Offset: 0x000A3630
		public virtual void TickOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				this.worldOverlayMat.SetTextureOffset("_MainTex", (float)Find.TickManager.TicksGame * this.worldPanDir1 * -1f * this.worldOverlayPanSpeed1 * this.worldOverlayMat.GetTextureScale("_MainTex").x);
				if (this.worldOverlayMat.HasProperty("_MainTex2"))
				{
					this.worldOverlayMat.SetTextureOffset("_MainTex2", (float)Find.TickManager.TicksGame * this.worldPanDir2 * -1f * this.worldOverlayPanSpeed2 * this.worldOverlayMat.GetTextureScale("_MainTex2").x);
				}
			}
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x000A5318 File Offset: 0x000A3718
		public void DrawOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				Vector3 position = map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather);
				Graphics.DrawMesh(MeshPool.wholeMapPlane, position, Quaternion.identity, this.worldOverlayMat, 0);
			}
			if (this.screenOverlayMat != null)
			{
				float num = Find.Camera.orthographicSize * 2f;
				Vector3 s = new Vector3(num * Find.Camera.aspect, 1f, num);
				Vector3 position2 = Find.Camera.transform.position;
				position2.y = AltitudeLayer.Weather.AltitudeFor() + 0.046875f;
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(position2, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.screenOverlayMat, 0);
			}
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x000A53F0 File Offset: 0x000A37F0
		public override string ToString()
		{
			string result;
			if (this.worldOverlayMat != null)
			{
				result = this.worldOverlayMat.name;
			}
			else if (this.screenOverlayMat != null)
			{
				result = this.screenOverlayMat.name;
			}
			else
			{
				result = "NoOverlayOverlay";
			}
			return result;
		}

		// Token: 0x04003096 RID: 12438
		public Material worldOverlayMat = null;

		// Token: 0x04003097 RID: 12439
		public Material screenOverlayMat = null;

		// Token: 0x04003098 RID: 12440
		protected float worldOverlayPanSpeed1;

		// Token: 0x04003099 RID: 12441
		protected float worldOverlayPanSpeed2;

		// Token: 0x0400309A RID: 12442
		protected Vector2 worldPanDir1;

		// Token: 0x0400309B RID: 12443
		protected Vector2 worldPanDir2;
	}
}
