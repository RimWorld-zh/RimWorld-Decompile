using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[AddComponentMenu("Image Effects/Displacement/Twirl")]
	[ExecuteInEditMode]
	public class Twirl : ImageEffectBase
	{
		public Vector2 radius = new Vector2(0.3f, 0.3f);

		[Range(0f, 360f)]
		public float angle = 50f;

		public Vector2 center = new Vector2(0.5f, 0.5f);

		public Twirl()
		{
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}
	}
}
