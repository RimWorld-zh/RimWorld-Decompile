using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200019A RID: 410
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
	public class Grayscale : ImageEffectBase
	{
		// Token: 0x04000804 RID: 2052
		public Texture textureRamp;

		// Token: 0x04000805 RID: 2053
		[Range(-1f, 1f)]
		public float rampOffset;

		// Token: 0x06000924 RID: 2340 RVA: 0x00016ABA File Offset: 0x00014CBA
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", this.textureRamp);
			base.material.SetFloat("_RampOffset", this.rampOffset);
			Graphics.Blit(source, destination, base.material);
		}
	}
}
