using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200019E RID: 414
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise And Grain (Filmic)")]
	public class NoiseAndGrain : PostEffectsBase
	{
		// Token: 0x06000931 RID: 2353 RVA: 0x00016E00 File Offset: 0x00015000
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.noiseMaterial = base.CheckShaderAndCreateMaterial(this.noiseShader, this.noiseMaterial);
			if (this.dx11Grain && this.supportDX11)
			{
				this.dx11NoiseMaterial = base.CheckShaderAndCreateMaterial(this.dx11NoiseShader, this.dx11NoiseMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00016E7C File Offset: 0x0001507C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || null == this.noiseTexture)
			{
				Graphics.Blit(source, destination);
				if (null == this.noiseTexture)
				{
					Debug.LogWarning("Noise & Grain effect failing as noise texture is not assigned. please assign.", base.transform);
				}
			}
			else
			{
				this.softness = Mathf.Clamp(this.softness, 0f, 0.99f);
				if (this.dx11Grain && this.supportDX11)
				{
					this.dx11NoiseMaterial.SetFloat("_DX11NoiseTime", (float)Time.frameCount);
					this.dx11NoiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
					this.dx11NoiseMaterial.SetVector("_NoisePerChannel", (!this.monochrome) ? this.intensities : Vector3.one);
					this.dx11NoiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
					this.dx11NoiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
					if (this.softness > Mathf.Epsilon)
					{
						RenderTexture temporary = RenderTexture.GetTemporary((int)((float)source.width * (1f - this.softness)), (int)((float)source.height * (1f - this.softness)));
						NoiseAndGrain.DrawNoiseQuadGrid(source, temporary, this.dx11NoiseMaterial, this.noiseTexture, (!this.monochrome) ? 2 : 3);
						this.dx11NoiseMaterial.SetTexture("_NoiseTex", temporary);
						Graphics.Blit(source, destination, this.dx11NoiseMaterial, 4);
						RenderTexture.ReleaseTemporary(temporary);
					}
					else
					{
						NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.dx11NoiseMaterial, this.noiseTexture, (!this.monochrome) ? 0 : 1);
					}
				}
				else
				{
					if (this.noiseTexture)
					{
						this.noiseTexture.wrapMode = TextureWrapMode.Repeat;
						this.noiseTexture.filterMode = this.filterMode;
					}
					this.noiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
					this.noiseMaterial.SetVector("_NoisePerChannel", (!this.monochrome) ? this.intensities : Vector3.one);
					this.noiseMaterial.SetVector("_NoiseTilingPerChannel", (!this.monochrome) ? this.tiling : (Vector3.one * this.monochromeTiling));
					this.noiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
					this.noiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
					if (this.softness > Mathf.Epsilon)
					{
						RenderTexture temporary2 = RenderTexture.GetTemporary((int)((float)source.width * (1f - this.softness)), (int)((float)source.height * (1f - this.softness)));
						NoiseAndGrain.DrawNoiseQuadGrid(source, temporary2, this.noiseMaterial, this.noiseTexture, 2);
						this.noiseMaterial.SetTexture("_NoiseTex", temporary2);
						Graphics.Blit(source, destination, this.noiseMaterial, 1);
						RenderTexture.ReleaseTemporary(temporary2);
					}
					else
					{
						NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.noiseMaterial, this.noiseTexture, 0);
					}
				}
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00017250 File Offset: 0x00015450
		private static void DrawNoiseQuadGrid(RenderTexture source, RenderTexture dest, Material fxMaterial, Texture2D noise, int passNr)
		{
			RenderTexture.active = dest;
			float num = (float)noise.width * 1f;
			float num2 = 1f * (float)source.width / NoiseAndGrain.TILE_AMOUNT;
			fxMaterial.SetTexture("_MainTex", source);
			GL.PushMatrix();
			GL.LoadOrtho();
			float num3 = 1f * (float)source.width / (1f * (float)source.height);
			float num4 = 1f / num2;
			float num5 = num4 * num3;
			float num6 = num / ((float)noise.width * 1f);
			fxMaterial.SetPass(passNr);
			GL.Begin(7);
			for (float num7 = 0f; num7 < 1f; num7 += num4)
			{
				for (float num8 = 0f; num8 < 1f; num8 += num5)
				{
					float num9 = UnityEngine.Random.Range(0f, 1f);
					float num10 = UnityEngine.Random.Range(0f, 1f);
					num9 = Mathf.Floor(num9 * num) / num;
					num10 = Mathf.Floor(num10 * num) / num;
					float num11 = 1f / num;
					GL.MultiTexCoord2(0, num9, num10);
					GL.MultiTexCoord2(1, 0f, 0f);
					GL.Vertex3(num7, num8, 0.1f);
					GL.MultiTexCoord2(0, num9 + num6 * num11, num10);
					GL.MultiTexCoord2(1, 1f, 0f);
					GL.Vertex3(num7 + num4, num8, 0.1f);
					GL.MultiTexCoord2(0, num9 + num6 * num11, num10 + num6 * num11);
					GL.MultiTexCoord2(1, 1f, 1f);
					GL.Vertex3(num7 + num4, num8 + num5, 0.1f);
					GL.MultiTexCoord2(0, num9, num10 + num6 * num11);
					GL.MultiTexCoord2(1, 0f, 1f);
					GL.Vertex3(num7, num8 + num5, 0.1f);
				}
			}
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0400080B RID: 2059
		public float intensityMultiplier = 0.25f;

		// Token: 0x0400080C RID: 2060
		public float generalIntensity = 0.5f;

		// Token: 0x0400080D RID: 2061
		public float blackIntensity = 1f;

		// Token: 0x0400080E RID: 2062
		public float whiteIntensity = 1f;

		// Token: 0x0400080F RID: 2063
		public float midGrey = 0.2f;

		// Token: 0x04000810 RID: 2064
		public bool dx11Grain = false;

		// Token: 0x04000811 RID: 2065
		public float softness = 0f;

		// Token: 0x04000812 RID: 2066
		public bool monochrome = false;

		// Token: 0x04000813 RID: 2067
		public Vector3 intensities = new Vector3(1f, 1f, 1f);

		// Token: 0x04000814 RID: 2068
		public Vector3 tiling = new Vector3(64f, 64f, 64f);

		// Token: 0x04000815 RID: 2069
		public float monochromeTiling = 64f;

		// Token: 0x04000816 RID: 2070
		public FilterMode filterMode = FilterMode.Bilinear;

		// Token: 0x04000817 RID: 2071
		public Texture2D noiseTexture;

		// Token: 0x04000818 RID: 2072
		public Shader noiseShader;

		// Token: 0x04000819 RID: 2073
		private Material noiseMaterial = null;

		// Token: 0x0400081A RID: 2074
		public Shader dx11NoiseShader;

		// Token: 0x0400081B RID: 2075
		private Material dx11NoiseMaterial = null;

		// Token: 0x0400081C RID: 2076
		private static float TILE_AMOUNT = 64f;
	}
}
