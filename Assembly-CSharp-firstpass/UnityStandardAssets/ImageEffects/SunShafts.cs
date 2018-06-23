using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001A9 RID: 425
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Sun Shafts")]
	public class SunShafts : PostEffectsBase
	{
		// Token: 0x04000856 RID: 2134
		public SunShafts.SunShaftsResolution resolution = SunShafts.SunShaftsResolution.Normal;

		// Token: 0x04000857 RID: 2135
		public SunShafts.ShaftsScreenBlendMode screenBlendMode = SunShafts.ShaftsScreenBlendMode.Screen;

		// Token: 0x04000858 RID: 2136
		public Transform sunTransform;

		// Token: 0x04000859 RID: 2137
		public int radialBlurIterations = 2;

		// Token: 0x0400085A RID: 2138
		public Color sunColor = Color.white;

		// Token: 0x0400085B RID: 2139
		public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);

		// Token: 0x0400085C RID: 2140
		public float sunShaftBlurRadius = 2.5f;

		// Token: 0x0400085D RID: 2141
		public float sunShaftIntensity = 1.15f;

		// Token: 0x0400085E RID: 2142
		public float maxRadius = 0.75f;

		// Token: 0x0400085F RID: 2143
		public bool useDepthTexture = true;

		// Token: 0x04000860 RID: 2144
		public Shader sunShaftsShader;

		// Token: 0x04000861 RID: 2145
		private Material sunShaftsMaterial;

		// Token: 0x04000862 RID: 2146
		public Shader simpleClearShader;

		// Token: 0x04000863 RID: 2147
		private Material simpleClearMaterial;

		// Token: 0x06000966 RID: 2406 RVA: 0x00018AF0 File Offset: 0x00016CF0
		public override bool CheckResources()
		{
			base.CheckSupport(this.useDepthTexture);
			this.sunShaftsMaterial = base.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
			this.simpleClearMaterial = base.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00018B5C File Offset: 0x00016D5C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				if (this.useDepthTexture)
				{
					base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
				}
				int num = 4;
				if (this.resolution == SunShafts.SunShaftsResolution.Normal)
				{
					num = 2;
				}
				else if (this.resolution == SunShafts.SunShaftsResolution.High)
				{
					num = 1;
				}
				Vector3 vector = Vector3.one * 0.5f;
				if (this.sunTransform)
				{
					vector = base.GetComponent<Camera>().WorldToViewportPoint(this.sunTransform.position);
				}
				else
				{
					vector = new Vector3(0.5f, 0.5f, 0f);
				}
				int width = source.width / num;
				int height = source.height / num;
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.sunShaftBlurRadius);
				this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
				this.sunShaftsMaterial.SetVector("_SunThreshold", this.sunThreshold);
				if (!this.useDepthTexture)
				{
					RenderTextureFormat format = (!base.GetComponent<Camera>().allowHDR) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
					RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, format);
					RenderTexture.active = temporary2;
					GL.ClearWithSkybox(false, base.GetComponent<Camera>());
					this.sunShaftsMaterial.SetTexture("_Skybox", temporary2);
					Graphics.Blit(source, temporary, this.sunShaftsMaterial, 3);
					RenderTexture.ReleaseTemporary(temporary2);
				}
				else
				{
					Graphics.Blit(source, temporary, this.sunShaftsMaterial, 2);
				}
				base.DrawBorder(temporary, this.simpleClearMaterial);
				this.radialBlurIterations = Mathf.Clamp(this.radialBlurIterations, 1, 4);
				float num2 = this.sunShaftBlurRadius * 0.00130208337f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
				this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
				for (int i = 0; i < this.radialBlurIterations; i++)
				{
					RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, 0);
					Graphics.Blit(temporary, temporary3, this.sunShaftsMaterial, 1);
					RenderTexture.ReleaseTemporary(temporary);
					num2 = this.sunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
					this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
					temporary = RenderTexture.GetTemporary(width, height, 0);
					Graphics.Blit(temporary3, temporary, this.sunShaftsMaterial, 1);
					RenderTexture.ReleaseTemporary(temporary3);
					num2 = this.sunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
					this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
				}
				if (vector.z >= 0f)
				{
					this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
				}
				else
				{
					this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
				}
				this.sunShaftsMaterial.SetTexture("_ColorBuffer", temporary);
				Graphics.Blit(source, destination, this.sunShaftsMaterial, (this.screenBlendMode != SunShafts.ShaftsScreenBlendMode.Screen) ? 4 : 0);
				RenderTexture.ReleaseTemporary(temporary);
			}
		}

		// Token: 0x020001AA RID: 426
		public enum SunShaftsResolution
		{
			// Token: 0x04000865 RID: 2149
			Low,
			// Token: 0x04000866 RID: 2150
			Normal,
			// Token: 0x04000867 RID: 2151
			High
		}

		// Token: 0x020001AB RID: 427
		public enum ShaftsScreenBlendMode
		{
			// Token: 0x04000869 RID: 2153
			Screen,
			// Token: 0x0400086A RID: 2154
			Add
		}
	}
}
