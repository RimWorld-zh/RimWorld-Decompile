using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200018B RID: 395
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Color Adjustments/Contrast Enhance (Unsharp Mask)")]
	public class ContrastEnhance : PostEffectsBase
	{
		// Token: 0x0400076F RID: 1903
		[Range(0f, 1f)]
		public float intensity = 0.5f;

		// Token: 0x04000770 RID: 1904
		[Range(0f, 0.999f)]
		public float threshold = 0f;

		// Token: 0x04000771 RID: 1905
		private Material separableBlurMaterial;

		// Token: 0x04000772 RID: 1906
		private Material contrastCompositeMaterial;

		// Token: 0x04000773 RID: 1907
		[Range(0f, 1f)]
		public float blurSpread = 1f;

		// Token: 0x04000774 RID: 1908
		public Shader separableBlurShader = null;

		// Token: 0x04000775 RID: 1909
		public Shader contrastCompositeShader = null;

		// Token: 0x060008ED RID: 2285 RVA: 0x00013824 File Offset: 0x00011A24
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.contrastCompositeMaterial = base.CheckShaderAndCreateMaterial(this.contrastCompositeShader, this.contrastCompositeMaterial);
			this.separableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00013888 File Offset: 0x00011A88
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				int width = source.width;
				int height = source.height;
				RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0);
				Graphics.Blit(source, temporary);
				RenderTexture temporary2 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
				Graphics.Blit(temporary, temporary2);
				RenderTexture.ReleaseTemporary(temporary);
				this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 1f / (float)temporary2.height, 0f, 0f));
				RenderTexture temporary3 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
				Graphics.Blit(temporary2, temporary3, this.separableBlurMaterial);
				RenderTexture.ReleaseTemporary(temporary2);
				this.separableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 1f / (float)temporary2.width, 0f, 0f, 0f));
				temporary2 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
				Graphics.Blit(temporary3, temporary2, this.separableBlurMaterial);
				RenderTexture.ReleaseTemporary(temporary3);
				this.contrastCompositeMaterial.SetTexture("_MainTexBlurred", temporary2);
				this.contrastCompositeMaterial.SetFloat("intensity", this.intensity);
				this.contrastCompositeMaterial.SetFloat("threshold", this.threshold);
				Graphics.Blit(source, destination, this.contrastCompositeMaterial);
				RenderTexture.ReleaseTemporary(temporary2);
			}
		}
	}
}
