using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
	[ExecuteInEditMode]
	public class SepiaTone : ImageEffectBase
	{
		public SepiaTone()
		{
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, base.material);
		}
	}
}
