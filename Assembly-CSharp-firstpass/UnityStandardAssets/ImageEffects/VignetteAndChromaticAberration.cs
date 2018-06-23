using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001B4 RID: 436
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
	public class VignetteAndChromaticAberration : PostEffectsBase
	{
		// Token: 0x0400089B RID: 2203
		public VignetteAndChromaticAberration.AberrationMode mode = VignetteAndChromaticAberration.AberrationMode.Simple;

		// Token: 0x0400089C RID: 2204
		public float intensity = 0.036f;

		// Token: 0x0400089D RID: 2205
		public float chromaticAberration = 0.2f;

		// Token: 0x0400089E RID: 2206
		public float axialAberration = 0.5f;

		// Token: 0x0400089F RID: 2207
		public float blur = 0f;

		// Token: 0x040008A0 RID: 2208
		public float blurSpread = 0.75f;

		// Token: 0x040008A1 RID: 2209
		public float luminanceDependency = 0.25f;

		// Token: 0x040008A2 RID: 2210
		public float blurDistance = 2.5f;

		// Token: 0x040008A3 RID: 2211
		public Shader vignetteShader;

		// Token: 0x040008A4 RID: 2212
		public Shader separableBlurShader;

		// Token: 0x040008A5 RID: 2213
		public Shader chromAberrationShader;

		// Token: 0x040008A6 RID: 2214
		private Material m_VignetteMaterial;

		// Token: 0x040008A7 RID: 2215
		private Material m_SeparableBlurMaterial;

		// Token: 0x040008A8 RID: 2216
		private Material m_ChromAberrationMaterial;

		// Token: 0x0600097A RID: 2426 RVA: 0x00019C5C File Offset: 0x00017E5C
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.m_VignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.m_VignetteMaterial);
			this.m_SeparableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.m_SeparableBlurMaterial);
			this.m_ChromAberrationMaterial = base.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.m_ChromAberrationMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00019CD8 File Offset: 0x00017ED8
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
				bool flag = Mathf.Abs(this.blur) > 0f || Mathf.Abs(this.intensity) > 0f;
				float num = 1f * (float)width / (1f * (float)height);
				RenderTexture renderTexture = null;
				RenderTexture renderTexture2 = null;
				if (flag)
				{
					renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
					if (Mathf.Abs(this.blur) > 0f)
					{
						renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(source, renderTexture2, this.m_ChromAberrationMaterial, 0);
						for (int i = 0; i < 2; i++)
						{
							this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 0.001953125f, 0f, 0f));
							RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
							Graphics.Blit(renderTexture2, temporary, this.m_SeparableBlurMaterial);
							RenderTexture.ReleaseTemporary(renderTexture2);
							this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 0.001953125f / num, 0f, 0f, 0f));
							renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
							Graphics.Blit(temporary, renderTexture2, this.m_SeparableBlurMaterial);
							RenderTexture.ReleaseTemporary(temporary);
						}
					}
					this.m_VignetteMaterial.SetFloat("_Intensity", 1f / (1f - this.intensity) - 1f);
					this.m_VignetteMaterial.SetFloat("_Blur", 1f / (1f - this.blur) - 1f);
					this.m_VignetteMaterial.SetTexture("_VignetteTex", renderTexture2);
					Graphics.Blit(source, renderTexture, this.m_VignetteMaterial, 0);
				}
				this.m_ChromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
				this.m_ChromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
				this.m_ChromAberrationMaterial.SetVector("_BlurDistance", new Vector2(-this.blurDistance, this.blurDistance));
				this.m_ChromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, this.luminanceDependency));
				if (flag)
				{
					renderTexture.wrapMode = TextureWrapMode.Clamp;
				}
				else
				{
					source.wrapMode = TextureWrapMode.Clamp;
				}
				Graphics.Blit((!flag) ? source : renderTexture, destination, this.m_ChromAberrationMaterial, (this.mode != VignetteAndChromaticAberration.AberrationMode.Advanced) ? 1 : 2);
				RenderTexture.ReleaseTemporary(renderTexture);
				RenderTexture.ReleaseTemporary(renderTexture2);
			}
		}

		// Token: 0x020001B5 RID: 437
		public enum AberrationMode
		{
			// Token: 0x040008AA RID: 2218
			Simple,
			// Token: 0x040008AB RID: 2219
			Advanced
		}
	}
}
