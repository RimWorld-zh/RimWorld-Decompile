using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200017F RID: 383
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
	public class BloomOptimized : PostEffectsBase
	{
		// Token: 0x04000710 RID: 1808
		[Range(0f, 1.5f)]
		public float threshold = 0.25f;

		// Token: 0x04000711 RID: 1809
		[Range(0f, 2.5f)]
		public float intensity = 0.75f;

		// Token: 0x04000712 RID: 1810
		[Range(0.25f, 5.5f)]
		public float blurSize = 1f;

		// Token: 0x04000713 RID: 1811
		private BloomOptimized.Resolution resolution = BloomOptimized.Resolution.Low;

		// Token: 0x04000714 RID: 1812
		[Range(1f, 4f)]
		public int blurIterations = 1;

		// Token: 0x04000715 RID: 1813
		public BloomOptimized.BlurType blurType = BloomOptimized.BlurType.Standard;

		// Token: 0x04000716 RID: 1814
		public Shader fastBloomShader = null;

		// Token: 0x04000717 RID: 1815
		private Material fastBloomMaterial = null;

		// Token: 0x060008C0 RID: 2240 RVA: 0x0001172C File Offset: 0x0000F92C
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fastBloomMaterial = base.CheckShaderAndCreateMaterial(this.fastBloomShader, this.fastBloomMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00011778 File Offset: 0x0000F978
		private void OnDisable()
		{
			if (this.fastBloomMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.fastBloomMaterial);
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00011798 File Offset: 0x0000F998
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				int num = (this.resolution != BloomOptimized.Resolution.Low) ? 2 : 4;
				float num2 = (this.resolution != BloomOptimized.Resolution.Low) ? 1f : 0.5f;
				this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0f, this.threshold, this.intensity));
				source.filterMode = FilterMode.Bilinear;
				int width = source.width / num;
				int height = source.height / num;
				RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
				renderTexture.filterMode = FilterMode.Bilinear;
				Graphics.Blit(source, renderTexture, this.fastBloomMaterial, 1);
				int num3 = (this.blurType != BloomOptimized.BlurType.Standard) ? 2 : 0;
				for (int i = 0; i < this.blurIterations; i++)
				{
					this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2 + (float)i * 1f, 0f, this.threshold, this.intensity));
					RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
					temporary.filterMode = FilterMode.Bilinear;
					Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 2 + num3);
					RenderTexture.ReleaseTemporary(renderTexture);
					renderTexture = temporary;
					temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
					temporary.filterMode = FilterMode.Bilinear;
					Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 3 + num3);
					RenderTexture.ReleaseTemporary(renderTexture);
					renderTexture = temporary;
				}
				this.fastBloomMaterial.SetTexture("_Bloom", renderTexture);
				Graphics.Blit(source, destination, this.fastBloomMaterial, 0);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x02000180 RID: 384
		public enum Resolution
		{
			// Token: 0x04000719 RID: 1817
			Low,
			// Token: 0x0400071A RID: 1818
			High
		}

		// Token: 0x02000181 RID: 385
		public enum BlurType
		{
			// Token: 0x0400071C RID: 1820
			Standard,
			// Token: 0x0400071D RID: 1821
			Sgx
		}
	}
}
