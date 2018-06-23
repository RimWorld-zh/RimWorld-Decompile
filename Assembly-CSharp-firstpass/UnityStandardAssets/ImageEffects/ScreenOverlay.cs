using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001A3 RID: 419
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Screen Overlay")]
	public class ScreenOverlay : PostEffectsBase
	{
		// Token: 0x04000834 RID: 2100
		public ScreenOverlay.OverlayBlendMode blendMode = ScreenOverlay.OverlayBlendMode.Overlay;

		// Token: 0x04000835 RID: 2101
		public float intensity = 1f;

		// Token: 0x04000836 RID: 2102
		public Texture2D texture = null;

		// Token: 0x04000837 RID: 2103
		public Shader overlayShader = null;

		// Token: 0x04000838 RID: 2104
		private Material overlayMaterial = null;

		// Token: 0x06000955 RID: 2389 RVA: 0x00018184 File Offset: 0x00016384
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.overlayMaterial = base.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000181D0 File Offset: 0x000163D0
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				Vector4 value = new Vector4(1f, 0f, 0f, 1f);
				this.overlayMaterial.SetVector("_UV_Transform", value);
				this.overlayMaterial.SetFloat("_Intensity", this.intensity);
				this.overlayMaterial.SetTexture("_Overlay", this.texture);
				Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
			}
		}

		// Token: 0x020001A4 RID: 420
		public enum OverlayBlendMode
		{
			// Token: 0x0400083A RID: 2106
			Additive,
			// Token: 0x0400083B RID: 2107
			ScreenBlend,
			// Token: 0x0400083C RID: 2108
			Multiply,
			// Token: 0x0400083D RID: 2109
			Overlay,
			// Token: 0x0400083E RID: 2110
			AlphaBlend
		}
	}
}
