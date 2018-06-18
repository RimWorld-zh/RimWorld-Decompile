using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200017E RID: 382
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/BloomAndFlares (3.5, Deprecated)")]
	public class BloomAndFlares : PostEffectsBase
	{
		// Token: 0x060008B9 RID: 2233 RVA: 0x00010D40 File Offset: 0x0000EF40
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
			this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
			this.vignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.vignetteMaterial);
			this.separableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
			this.addBrightStuffBlendOneOneMaterial = base.CheckShaderAndCreateMaterial(this.addBrightStuffOneOneShader, this.addBrightStuffBlendOneOneMaterial);
			this.hollywoodFlaresMaterial = base.CheckShaderAndCreateMaterial(this.hollywoodFlaresShader, this.hollywoodFlaresMaterial);
			this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00010E1C File Offset: 0x0000F01C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				this.doHdr = false;
				if (this.hdr == HDRBloomMode.Auto)
				{
					this.doHdr = (source.format == RenderTextureFormat.ARGBHalf && base.GetComponent<Camera>().allowHDR);
				}
				else
				{
					this.doHdr = (this.hdr == HDRBloomMode.On);
				}
				this.doHdr = (this.doHdr && this.supportHDRTextures);
				BloomScreenBlendMode pass = this.screenBlendMode;
				if (this.doHdr)
				{
					pass = BloomScreenBlendMode.Add;
				}
				RenderTextureFormat format = (!this.doHdr) ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf;
				RenderTexture temporary = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, format);
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
				RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
				RenderTexture temporary4 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
				float num = 1f * (float)source.width / (1f * (float)source.height);
				float num2 = 0.001953125f;
				Graphics.Blit(source, temporary, this.screenBlend, 2);
				Graphics.Blit(temporary, temporary2, this.screenBlend, 2);
				RenderTexture.ReleaseTemporary(temporary);
				this.BrightFilter(this.bloomThreshold, this.useSrcAlphaAsMask, temporary2, temporary3);
				temporary2.DiscardContents();
				if (this.bloomBlurIterations < 1)
				{
					this.bloomBlurIterations = 1;
				}
				for (int i = 0; i < this.bloomBlurIterations; i++)
				{
					float num3 = (1f + (float)i * 0.5f) * this.sepBlurSpread;
					this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, num3 * num2, 0f, 0f));
					RenderTexture renderTexture = (i != 0) ? temporary2 : temporary3;
					Graphics.Blit(renderTexture, temporary4, this.separableBlurMaterial);
					renderTexture.DiscardContents();
					this.separableBlurMaterial.SetVector("offsets", new Vector4(num3 / num * num2, 0f, 0f, 0f));
					Graphics.Blit(temporary4, temporary2, this.separableBlurMaterial);
					temporary4.DiscardContents();
				}
				if (this.lensflares)
				{
					if (this.lensflareMode == LensflareStyle34.Ghosting)
					{
						this.BrightFilter(this.lensflareThreshold, 0f, temporary2, temporary4);
						temporary2.DiscardContents();
						this.Vignette(0.975f, temporary4, temporary3);
						temporary4.DiscardContents();
						this.BlendFlares(temporary3, temporary2);
						temporary3.DiscardContents();
					}
					else
					{
						this.hollywoodFlaresMaterial.SetVector("_threshold", new Vector4(this.lensflareThreshold, 1f / (1f - this.lensflareThreshold), 0f, 0f));
						this.hollywoodFlaresMaterial.SetVector("tintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
						Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 2);
						temporary4.DiscardContents();
						Graphics.Blit(temporary3, temporary4, this.hollywoodFlaresMaterial, 3);
						temporary3.DiscardContents();
						this.hollywoodFlaresMaterial.SetVector("offsets", new Vector4(this.sepBlurSpread * 1f / num * num2, 0f, 0f, 0f));
						this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth);
						Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
						temporary4.DiscardContents();
						this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 2f);
						Graphics.Blit(temporary3, temporary4, this.hollywoodFlaresMaterial, 1);
						temporary3.DiscardContents();
						this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 4f);
						Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
						temporary4.DiscardContents();
						if (this.lensflareMode == LensflareStyle34.Anamorphic)
						{
							for (int j = 0; j < this.hollywoodFlareBlurIterations; j++)
							{
								this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
								Graphics.Blit(temporary3, temporary4, this.separableBlurMaterial);
								temporary3.DiscardContents();
								this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
								Graphics.Blit(temporary4, temporary3, this.separableBlurMaterial);
								temporary4.DiscardContents();
							}
							this.AddTo(1f, temporary3, temporary2);
							temporary3.DiscardContents();
						}
						else
						{
							for (int k = 0; k < this.hollywoodFlareBlurIterations; k++)
							{
								this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
								Graphics.Blit(temporary3, temporary4, this.separableBlurMaterial);
								temporary3.DiscardContents();
								this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
								Graphics.Blit(temporary4, temporary3, this.separableBlurMaterial);
								temporary4.DiscardContents();
							}
							this.Vignette(1f, temporary3, temporary4);
							temporary3.DiscardContents();
							this.BlendFlares(temporary4, temporary3);
							temporary4.DiscardContents();
							this.AddTo(1f, temporary3, temporary2);
							temporary3.DiscardContents();
						}
					}
				}
				this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
				this.screenBlend.SetTexture("_ColorBuffer", source);
				Graphics.Blit(temporary2, destination, this.screenBlend, (int)pass);
				RenderTexture.ReleaseTemporary(temporary2);
				RenderTexture.ReleaseTemporary(temporary3);
				RenderTexture.ReleaseTemporary(temporary4);
			}
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0001146B File Offset: 0x0000F66B
		private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
		{
			this.addBrightStuffBlendOneOneMaterial.SetFloat("_Intensity", intensity_);
			Graphics.Blit(from, to, this.addBrightStuffBlendOneOneMaterial);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0001148C File Offset: 0x0000F68C
		private void BlendFlares(RenderTexture from, RenderTexture to)
		{
			this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
			Graphics.Blit(from, to, this.lensFlareMaterial);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x000115D8 File Offset: 0x0000F7D8
		private void BrightFilter(float thresh, float useAlphaAsMask, RenderTexture from, RenderTexture to)
		{
			if (this.doHdr)
			{
				this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f, 0f, 0f));
			}
			else
			{
				this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f / (1f - thresh), 0f, 0f));
			}
			this.brightPassFilterMaterial.SetFloat("useSrcAlphaAsMask", useAlphaAsMask);
			Graphics.Blit(from, to, this.brightPassFilterMaterial);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00011668 File Offset: 0x0000F868
		private void Vignette(float amount, RenderTexture from, RenderTexture to)
		{
			if (this.lensFlareVignetteMask)
			{
				this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
				Graphics.Blit(from, to, this.screenBlend, 3);
			}
			else
			{
				this.vignetteMaterial.SetFloat("vignetteIntensity", amount);
				Graphics.Blit(from, to, this.vignetteMaterial);
			}
		}

		// Token: 0x040006EE RID: 1774
		public TweakMode34 tweakMode = TweakMode34.Basic;

		// Token: 0x040006EF RID: 1775
		public BloomScreenBlendMode screenBlendMode = BloomScreenBlendMode.Add;

		// Token: 0x040006F0 RID: 1776
		public HDRBloomMode hdr = HDRBloomMode.Auto;

		// Token: 0x040006F1 RID: 1777
		private bool doHdr = false;

		// Token: 0x040006F2 RID: 1778
		public float sepBlurSpread = 1.5f;

		// Token: 0x040006F3 RID: 1779
		public float useSrcAlphaAsMask = 0.5f;

		// Token: 0x040006F4 RID: 1780
		public float bloomIntensity = 1f;

		// Token: 0x040006F5 RID: 1781
		public float bloomThreshold = 0.5f;

		// Token: 0x040006F6 RID: 1782
		public int bloomBlurIterations = 2;

		// Token: 0x040006F7 RID: 1783
		public bool lensflares = false;

		// Token: 0x040006F8 RID: 1784
		public int hollywoodFlareBlurIterations = 2;

		// Token: 0x040006F9 RID: 1785
		public LensflareStyle34 lensflareMode = LensflareStyle34.Anamorphic;

		// Token: 0x040006FA RID: 1786
		public float hollyStretchWidth = 3.5f;

		// Token: 0x040006FB RID: 1787
		public float lensflareIntensity = 1f;

		// Token: 0x040006FC RID: 1788
		public float lensflareThreshold = 0.3f;

		// Token: 0x040006FD RID: 1789
		public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);

		// Token: 0x040006FE RID: 1790
		public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);

		// Token: 0x040006FF RID: 1791
		public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04000700 RID: 1792
		public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);

		// Token: 0x04000701 RID: 1793
		public Texture2D lensFlareVignetteMask;

		// Token: 0x04000702 RID: 1794
		public Shader lensFlareShader;

		// Token: 0x04000703 RID: 1795
		private Material lensFlareMaterial;

		// Token: 0x04000704 RID: 1796
		public Shader vignetteShader;

		// Token: 0x04000705 RID: 1797
		private Material vignetteMaterial;

		// Token: 0x04000706 RID: 1798
		public Shader separableBlurShader;

		// Token: 0x04000707 RID: 1799
		private Material separableBlurMaterial;

		// Token: 0x04000708 RID: 1800
		public Shader addBrightStuffOneOneShader;

		// Token: 0x04000709 RID: 1801
		private Material addBrightStuffBlendOneOneMaterial;

		// Token: 0x0400070A RID: 1802
		public Shader screenBlendShader;

		// Token: 0x0400070B RID: 1803
		private Material screenBlend;

		// Token: 0x0400070C RID: 1804
		public Shader hollywoodFlaresShader;

		// Token: 0x0400070D RID: 1805
		private Material hollywoodFlaresMaterial;

		// Token: 0x0400070E RID: 1806
		public Shader brightPassFilterShader;

		// Token: 0x0400070F RID: 1807
		private Material brightPassFilterMaterial;
	}
}
