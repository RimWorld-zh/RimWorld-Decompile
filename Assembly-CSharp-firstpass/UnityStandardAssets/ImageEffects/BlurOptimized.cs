using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000183 RID: 387
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
	public class BlurOptimized : PostEffectsBase
	{
		// Token: 0x060008CC RID: 2252 RVA: 0x00011BE8 File Offset: 0x0000FDE8
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00011C34 File Offset: 0x0000FE34
		public void OnDisable()
		{
			if (this.blurMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.blurMaterial);
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00011C54 File Offset: 0x0000FE54
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				float num = 1f / (1f * (float)(1 << this.downsample));
				this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num, -this.blurSize * num, 0f, 0f));
				source.filterMode = FilterMode.Bilinear;
				int width = source.width >> this.downsample;
				int height = source.height >> this.downsample;
				RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
				renderTexture.filterMode = FilterMode.Bilinear;
				Graphics.Blit(source, renderTexture, this.blurMaterial, 0);
				int num2 = (this.blurType != BlurOptimized.BlurType.StandardGauss) ? 2 : 0;
				for (int i = 0; i < this.blurIterations; i++)
				{
					float num3 = (float)i * 1f;
					this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num + num3, -this.blurSize * num - num3, 0f, 0f));
					RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
					temporary.filterMode = FilterMode.Bilinear;
					Graphics.Blit(renderTexture, temporary, this.blurMaterial, 1 + num2);
					RenderTexture.ReleaseTemporary(renderTexture);
					renderTexture = temporary;
					temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
					temporary.filterMode = FilterMode.Bilinear;
					Graphics.Blit(renderTexture, temporary, this.blurMaterial, 2 + num2);
					RenderTexture.ReleaseTemporary(renderTexture);
					renderTexture = temporary;
				}
				Graphics.Blit(renderTexture, destination);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x04000722 RID: 1826
		[Range(0f, 2f)]
		public int downsample = 1;

		// Token: 0x04000723 RID: 1827
		[Range(0f, 10f)]
		public float blurSize = 3f;

		// Token: 0x04000724 RID: 1828
		[Range(1f, 4f)]
		public int blurIterations = 2;

		// Token: 0x04000725 RID: 1829
		public BlurOptimized.BlurType blurType = BlurOptimized.BlurType.StandardGauss;

		// Token: 0x04000726 RID: 1830
		public Shader blurShader = null;

		// Token: 0x04000727 RID: 1831
		private Material blurMaterial = null;

		// Token: 0x02000184 RID: 388
		public enum BlurType
		{
			// Token: 0x04000729 RID: 1833
			StandardGauss,
			// Token: 0x0400072A RID: 1834
			SgxGauss
		}
	}
}
