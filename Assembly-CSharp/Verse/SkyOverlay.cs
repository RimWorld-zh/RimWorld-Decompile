using UnityEngine;

namespace Verse
{
	public abstract class SkyOverlay
	{
		public Material worldOverlayMat;

		public Material screenOverlayMat;

		protected float worldOverlayPanSpeed1;

		protected float worldOverlayPanSpeed2;

		protected Vector2 worldPanDir1;

		protected Vector2 worldPanDir2;

		public Color OverlayColor
		{
			set
			{
				if ((Object)this.worldOverlayMat != (Object)null)
				{
					this.worldOverlayMat.color = value;
				}
				if ((Object)this.screenOverlayMat != (Object)null)
				{
					this.screenOverlayMat.color = value;
				}
			}
		}

		public SkyOverlay()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.OverlayColor = Color.clear;
			});
		}

		public virtual void TickOverlay(Map map)
		{
			if ((Object)this.worldOverlayMat != (Object)null)
			{
				Material material = this.worldOverlayMat;
				Vector2 a = (float)Find.TickManager.TicksGame * this.worldPanDir1 * -1f * this.worldOverlayPanSpeed1;
				Vector2 textureScale = this.worldOverlayMat.GetTextureScale("_MainTex");
				material.SetTextureOffset("_MainTex", a * textureScale.x);
				if (this.worldOverlayMat.HasProperty("_MainTex2"))
				{
					Material material2 = this.worldOverlayMat;
					Vector2 a2 = (float)Find.TickManager.TicksGame * this.worldPanDir2 * -1f * this.worldOverlayPanSpeed2;
					Vector2 textureScale2 = this.worldOverlayMat.GetTextureScale("_MainTex2");
					material2.SetTextureOffset("_MainTex2", a2 * textureScale2.x);
				}
			}
		}

		public void DrawOverlay(Map map)
		{
			if ((Object)this.worldOverlayMat != (Object)null)
			{
				Vector3 position = map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather);
				Graphics.DrawMesh(MeshPool.wholeMapPlane, position, Quaternion.identity, this.worldOverlayMat, 0);
			}
			if ((Object)this.screenOverlayMat != (Object)null)
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
			if ((Object)this.worldOverlayMat != (Object)null)
			{
				return this.worldOverlayMat.name;
			}
			if ((Object)this.screenOverlayMat != (Object)null)
			{
				return this.screenOverlayMat.name;
			}
			return "NoOverlayOverlay";
		}
	}
}
