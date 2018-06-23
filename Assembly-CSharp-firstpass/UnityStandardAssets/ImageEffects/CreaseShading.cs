using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200018D RID: 397
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Crease Shading")]
	public class CreaseShading : PostEffectsBase
	{
		// Token: 0x04000783 RID: 1923
		public float intensity = 0.5f;

		// Token: 0x04000784 RID: 1924
		public int softness = 1;

		// Token: 0x04000785 RID: 1925
		public float spread = 1f;

		// Token: 0x04000786 RID: 1926
		public Shader blurShader = null;

		// Token: 0x04000787 RID: 1927
		private Material blurMaterial = null;

		// Token: 0x04000788 RID: 1928
		public Shader depthFetchShader = null;

		// Token: 0x04000789 RID: 1929
		private Material depthFetchMaterial = null;

		// Token: 0x0400078A RID: 1930
		public Shader creaseApplyShader = null;

		// Token: 0x0400078B RID: 1931
		private Material creaseApplyMaterial = null;

		// Token: 0x060008FA RID: 2298 RVA: 0x00013EBC File Offset: 0x000120BC
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			this.depthFetchMaterial = base.CheckShaderAndCreateMaterial(this.depthFetchShader, this.depthFetchMaterial);
			this.creaseApplyMaterial = base.CheckShaderAndCreateMaterial(this.creaseApplyShader, this.creaseApplyMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00013F38 File Offset: 0x00012138
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
				float num = 1f * (float)width / (1f * (float)height);
				float num2 = 0.001953125f;
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
				RenderTexture renderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0);
				Graphics.Blit(source, temporary, this.depthFetchMaterial);
				Graphics.Blit(temporary, renderTexture);
				for (int i = 0; i < this.softness; i++)
				{
					RenderTexture temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
					this.blurMaterial.SetVector("offsets", new Vector4(0f, this.spread * num2, 0f, 0f));
					Graphics.Blit(renderTexture, temporary2, this.blurMaterial);
					RenderTexture.ReleaseTemporary(renderTexture);
					renderTexture = temporary2;
					temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
					this.blurMaterial.SetVector("offsets", new Vector4(this.spread * num2 / num, 0f, 0f, 0f));
					Graphics.Blit(renderTexture, temporary2, this.blurMaterial);
					RenderTexture.ReleaseTemporary(renderTexture);
					renderTexture = temporary2;
				}
				this.creaseApplyMaterial.SetTexture("_HrDepthTex", temporary);
				this.creaseApplyMaterial.SetTexture("_LrDepthTex", renderTexture);
				this.creaseApplyMaterial.SetFloat("intensity", this.intensity);
				Graphics.Blit(source, destination, this.creaseApplyMaterial);
				RenderTexture.ReleaseTemporary(temporary);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}
	}
}
