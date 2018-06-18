using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000187 RID: 391
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Curves, Saturation)")]
	public class ColorCorrectionCurves : PostEffectsBase
	{
		// Token: 0x060008DC RID: 2268 RVA: 0x00012DC2 File Offset: 0x00010FC2
		private new void Start()
		{
			base.Start();
			this.updateTexturesOnStartup = true;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00012DD2 File Offset: 0x00010FD2
		private void Awake()
		{
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00012DD8 File Offset: 0x00010FD8
		public override bool CheckResources()
		{
			base.CheckSupport(this.mode == ColorCorrectionCurves.ColorCorrectionMode.Advanced);
			this.ccMaterial = base.CheckShaderAndCreateMaterial(this.simpleColorCorrectionCurvesShader, this.ccMaterial);
			this.ccDepthMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionCurvesShader, this.ccDepthMaterial);
			this.selectiveCcMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionSelectiveShader, this.selectiveCcMaterial);
			if (!this.rgbChannelTex)
			{
				this.rgbChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!this.rgbDepthChannelTex)
			{
				this.rgbDepthChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!this.zCurveTex)
			{
				this.zCurveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
			}
			this.rgbChannelTex.hideFlags = HideFlags.DontSave;
			this.rgbDepthChannelTex.hideFlags = HideFlags.DontSave;
			this.zCurveTex.hideFlags = HideFlags.DontSave;
			this.rgbChannelTex.wrapMode = TextureWrapMode.Clamp;
			this.rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp;
			this.zCurveTex.wrapMode = TextureWrapMode.Clamp;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00012F14 File Offset: 0x00011114
		public void UpdateParameters()
		{
			this.CheckResources();
			if (this.redChannel != null && this.greenChannel != null && this.blueChannel != null)
			{
				for (float num = 0f; num <= 1f; num += 0.003921569f)
				{
					float num2 = Mathf.Clamp(this.redChannel.Evaluate(num), 0f, 1f);
					float num3 = Mathf.Clamp(this.greenChannel.Evaluate(num), 0f, 1f);
					float num4 = Mathf.Clamp(this.blueChannel.Evaluate(num), 0f, 1f);
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
					float num5 = Mathf.Clamp(this.zCurve.Evaluate(num), 0f, 1f);
					this.zCurveTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num5, num5, num5));
					num2 = Mathf.Clamp(this.depthRedChannel.Evaluate(num), 0f, 1f);
					num3 = Mathf.Clamp(this.depthGreenChannel.Evaluate(num), 0f, 1f);
					num4 = Mathf.Clamp(this.depthBlueChannel.Evaluate(num), 0f, 1f);
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
				}
				this.rgbChannelTex.Apply();
				this.rgbDepthChannelTex.Apply();
				this.zCurveTex.Apply();
			}
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001313C File Offset: 0x0001133C
		private void UpdateTextures()
		{
			this.UpdateParameters();
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00013148 File Offset: 0x00011348
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				if (this.updateTexturesOnStartup)
				{
					this.UpdateParameters();
					this.updateTexturesOnStartup = false;
				}
				if (this.useDepthCorrection)
				{
					base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
				}
				RenderTexture renderTexture = destination;
				if (this.selectiveCc)
				{
					renderTexture = RenderTexture.GetTemporary(source.width, source.height);
				}
				if (this.useDepthCorrection)
				{
					this.ccDepthMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
					this.ccDepthMaterial.SetTexture("_ZCurve", this.zCurveTex);
					this.ccDepthMaterial.SetTexture("_RgbDepthTex", this.rgbDepthChannelTex);
					this.ccDepthMaterial.SetFloat("_Saturation", this.saturation);
					Graphics.Blit(source, renderTexture, this.ccDepthMaterial);
				}
				else
				{
					this.ccMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
					this.ccMaterial.SetFloat("_Saturation", this.saturation);
					Graphics.Blit(source, renderTexture, this.ccMaterial);
				}
				if (this.selectiveCc)
				{
					this.selectiveCcMaterial.SetColor("selColor", this.selectiveFromColor);
					this.selectiveCcMaterial.SetColor("targetColor", this.selectiveToColor);
					Graphics.Blit(renderTexture, destination, this.selectiveCcMaterial);
					RenderTexture.ReleaseTemporary(renderTexture);
				}
			}
		}

		// Token: 0x0400074F RID: 1871
		public AnimationCurve redChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000750 RID: 1872
		public AnimationCurve greenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000751 RID: 1873
		public AnimationCurve blueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000752 RID: 1874
		public bool useDepthCorrection = false;

		// Token: 0x04000753 RID: 1875
		public AnimationCurve zCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000754 RID: 1876
		public AnimationCurve depthRedChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000755 RID: 1877
		public AnimationCurve depthGreenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000756 RID: 1878
		public AnimationCurve depthBlueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000757 RID: 1879
		private Material ccMaterial;

		// Token: 0x04000758 RID: 1880
		private Material ccDepthMaterial;

		// Token: 0x04000759 RID: 1881
		private Material selectiveCcMaterial;

		// Token: 0x0400075A RID: 1882
		private Texture2D rgbChannelTex;

		// Token: 0x0400075B RID: 1883
		private Texture2D rgbDepthChannelTex;

		// Token: 0x0400075C RID: 1884
		private Texture2D zCurveTex;

		// Token: 0x0400075D RID: 1885
		public float saturation = 1f;

		// Token: 0x0400075E RID: 1886
		public bool selectiveCc = false;

		// Token: 0x0400075F RID: 1887
		public Color selectiveFromColor = Color.white;

		// Token: 0x04000760 RID: 1888
		public Color selectiveToColor = Color.white;

		// Token: 0x04000761 RID: 1889
		public ColorCorrectionCurves.ColorCorrectionMode mode;

		// Token: 0x04000762 RID: 1890
		public bool updateTextures = true;

		// Token: 0x04000763 RID: 1891
		public Shader colorCorrectionCurvesShader = null;

		// Token: 0x04000764 RID: 1892
		public Shader simpleColorCorrectionCurvesShader = null;

		// Token: 0x04000765 RID: 1893
		public Shader colorCorrectionSelectiveShader = null;

		// Token: 0x04000766 RID: 1894
		private bool updateTexturesOnStartup = true;

		// Token: 0x02000188 RID: 392
		public enum ColorCorrectionMode
		{
			// Token: 0x04000768 RID: 1896
			Simple,
			// Token: 0x04000769 RID: 1897
			Advanced
		}
	}
}
