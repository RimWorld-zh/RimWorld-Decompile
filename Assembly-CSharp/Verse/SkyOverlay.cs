using System;
using UnityEngine;

namespace Verse
{
	public abstract class SkyOverlay
	{
		public Material worldOverlayMat = null;

		public Material screenOverlayMat = null;

		protected float worldOverlayPanSpeed1;

		protected float worldOverlayPanSpeed2;

		protected Vector2 worldPanDir1;

		protected Vector2 worldPanDir2;

		public Color OverlayColor
		{
			set
			{
				if ((UnityEngine.Object)this.worldOverlayMat != (UnityEngine.Object)null)
				{
					this.worldOverlayMat.color = value;
				}
				if ((UnityEngine.Object)this.screenOverlayMat != (UnityEngine.Object)null)
				{
					this.screenOverlayMat.color = value;
				}
			}
		}

		public SkyOverlay()
		{
			LongEventHandler.ExecuteWhenFinished((Action)delegate
			{
				this.OverlayColor = Color.clear;
			});
		}

		public virtual void TickOverlay(Map map)
		{
			if ((UnityEngine.Object)this.worldOverlayMat != (UnityEngine.Object)null)
			{
				Material obj = this.worldOverlayMat;
				Vector2 a = (float)Find.TickManager.TicksGame * this.worldPanDir1 * -1f * this.worldOverlayPanSpeed1;
				Vector2 textureScale = this.worldOverlayMat.GetTextureScale("_MainTex");
				obj.SetTextureOffset("_MainTex", a * textureScale.x);
				if (this.worldOverlayMat.HasProperty("_MainTex2"))
				{
					Material obj2 = this.worldOverlayMat;
					Vector2 a2 = (float)Find.TickManager.TicksGame * this.worldPanDir2 * -1f * this.worldOverlayPanSpeed2;
					Vector2 textureScale2 = this.worldOverlayMat.GetTextureScale("_MainTex2");
					obj2.SetTextureOffset("_MainTex2", a2 * textureScale2.x);
				}
			}
		}

		public void DrawOverlay(Map map)
		{
			if ((UnityEngine.Object)this.worldOverlayMat != (UnityEngine.Object)null)
			{
				Vector3 position = map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather);
				Graphics.DrawMesh(MeshPool.wholeMapPlane, position, Quaternion.identity, this.worldOverlayMat, 0);
			}
			if ((UnityEngine.Object)this.screenOverlayMat != (UnityEngine.Object)null)
			{
				float num = (float)(Find.Camera.orthographicSize * 2.0);
				Vector3 s = new Vector3(num * Find.Camera.aspect, 1f, num);
				Vector3 position2 = Find.Camera.transform.position;
				position2.y = (float)(Altitudes.AltitudeFor(AltitudeLayer.Weather) + 0.046875);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(position2, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.screenOverlayMat, 0);
			}
		}

		public override string ToString()
		{
			return (!((UnityEngine.Object)this.worldOverlayMat != (UnityEngine.Object)null)) ? ((!((UnityEngine.Object)this.screenOverlayMat != (UnityEngine.Object)null)) ? "NoOverlayOverlay" : this.screenOverlayMat.name) : this.worldOverlayMat.name;
		}
	}
}
