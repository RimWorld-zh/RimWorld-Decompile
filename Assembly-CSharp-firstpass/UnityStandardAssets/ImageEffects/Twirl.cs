using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001B3 RID: 435
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Twirl")]
	public class Twirl : ImageEffectBase
	{
		// Token: 0x06000978 RID: 2424 RVA: 0x00019BCF File Offset: 0x00017DCF
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x04000898 RID: 2200
		public Vector2 radius = new Vector2(0.3f, 0.3f);

		// Token: 0x04000899 RID: 2201
		[Range(0f, 360f)]
		public float angle = 50f;

		// Token: 0x0400089A RID: 2202
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
