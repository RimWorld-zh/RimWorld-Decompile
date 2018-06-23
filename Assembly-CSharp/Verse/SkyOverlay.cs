using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB0 RID: 3248
	public abstract class SkyOverlay
	{
		// Token: 0x0400309F RID: 12447
		public Material worldOverlayMat = null;

		// Token: 0x040030A0 RID: 12448
		public Material screenOverlayMat = null;

		// Token: 0x040030A1 RID: 12449
		protected float worldOverlayPanSpeed1;

		// Token: 0x040030A2 RID: 12450
		protected float worldOverlayPanSpeed2;

		// Token: 0x040030A3 RID: 12451
		protected Vector2 worldPanDir1;

		// Token: 0x040030A4 RID: 12452
		protected Vector2 worldPanDir2;

		// Token: 0x060047A3 RID: 18339 RVA: 0x000A51E7 File Offset: 0x000A35E7
		public SkyOverlay()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.OverlayColor = Color.clear;
			});
		}

		// Token: 0x17000B4D RID: 2893
		// (set) Token: 0x060047A4 RID: 18340 RVA: 0x000A520F File Offset: 0x000A360F
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

		// Token: 0x060047A5 RID: 18341 RVA: 0x000A524C File Offset: 0x000A364C
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

		// Token: 0x060047A6 RID: 18342 RVA: 0x000A5334 File Offset: 0x000A3734
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

		// Token: 0x060047A7 RID: 18343 RVA: 0x000A540C File Offset: 0x000A380C
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
	}
}
