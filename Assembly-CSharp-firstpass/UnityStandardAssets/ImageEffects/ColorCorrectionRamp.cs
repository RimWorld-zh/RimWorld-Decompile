using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200018A RID: 394
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")]
	public class ColorCorrectionRamp : ImageEffectBase
	{
		// Token: 0x060008EB RID: 2283 RVA: 0x000137C4 File Offset: 0x000119C4
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", this.textureRamp);
			Graphics.Blit(source, destination, base.material);
		}

		// Token: 0x0400076E RID: 1902
		public Texture textureRamp;
	}
}
