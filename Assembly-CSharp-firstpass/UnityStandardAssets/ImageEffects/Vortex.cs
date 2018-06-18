using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001B6 RID: 438
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Vortex")]
	public class Vortex : ImageEffectBase
	{
		// Token: 0x0600097D RID: 2429 RVA: 0x00019FEB File Offset: 0x000181EB
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x040008AC RID: 2220
		public Vector2 radius = new Vector2(0.4f, 0.4f);

		// Token: 0x040008AD RID: 2221
		public float angle = 50f;

		// Token: 0x040008AE RID: 2222
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
