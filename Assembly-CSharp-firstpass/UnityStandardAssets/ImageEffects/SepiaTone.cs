using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001A8 RID: 424
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
	public class SepiaTone : ImageEffectBase
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x00018A66 File Offset: 0x00016C66
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, base.material);
		}
	}
}
