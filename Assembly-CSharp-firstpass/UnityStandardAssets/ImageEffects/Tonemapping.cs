using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001AF RID: 431
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Color Adjustments/Tonemapping")]
	public class Tonemapping : PostEffectsBase
	{
		// Token: 0x0600096C RID: 2412 RVA: 0x00019198 File Offset: 0x00017398
		public override bool CheckResources()
		{
			base.CheckSupport(false, true);
			this.tonemapMaterial = base.CheckShaderAndCreateMaterial(this.tonemapper, this.tonemapMaterial);
			if (!this.curveTex && this.type == Tonemapping.TonemapperType.UserCurve)
			{
				this.curveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
				this.curveTex.filterMode = FilterMode.Bilinear;
				this.curveTex.wrapMode = TextureWrapMode.Clamp;
				this.curveTex.hideFlags = HideFlags.DontSave;
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001923C File Offset: 0x0001743C
		public float UpdateCurve()
		{
			float num = 1f;
			if (this.remapCurve.keys.Length < 1)
			{
				this.remapCurve = new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(2f, 1f)
				});
			}
			if (this.remapCurve != null)
			{
				if (this.remapCurve.length > 0)
				{
					num = this.remapCurve[this.remapCurve.length - 1].time;
				}
				for (float num2 = 0f; num2 <= 1f; num2 += 0.003921569f)
				{
					float num3 = this.remapCurve.Evaluate(num2 * 1f * num);
					this.curveTex.SetPixel((int)Mathf.Floor(num2 * 255f), 0, new Color(num3, num3, num3));
				}
				this.curveTex.Apply();
			}
			return 1f / num;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0001935C File Offset: 0x0001755C
		private void OnDisable()
		{
			if (this.rt)
			{
				UnityEngine.Object.DestroyImmediate(this.rt);
				this.rt = null;
			}
			if (this.tonemapMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.tonemapMaterial);
				this.tonemapMaterial = null;
			}
			if (this.curveTex)
			{
				UnityEngine.Object.DestroyImmediate(this.curveTex);
				this.curveTex = null;
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000193D8 File Offset: 0x000175D8
		private bool CreateInternalRenderTexture()
		{
			bool result;
			if (this.rt)
			{
				result = false;
			}
			else
			{
				this.rtFormat = ((!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf)) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.RGHalf);
				this.rt = new RenderTexture(1, 1, 0, this.rtFormat);
				this.rt.hideFlags = HideFlags.DontSave;
				result = true;
			}
			return result;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00019444 File Offset: 0x00017644
		[ImageEffectTransformsToLDR]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				this.exposureAdjustment = ((this.exposureAdjustment >= 0.001f) ? this.exposureAdjustment : 0.001f);
				if (this.type == Tonemapping.TonemapperType.UserCurve)
				{
					float value = this.UpdateCurve();
					this.tonemapMaterial.SetFloat("_RangeScale", value);
					this.tonemapMaterial.SetTexture("_Curve", this.curveTex);
					Graphics.Blit(source, destination, this.tonemapMaterial, 4);
				}
				else if (this.type == Tonemapping.TonemapperType.SimpleReinhard)
				{
					this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
					Graphics.Blit(source, destination, this.tonemapMaterial, 6);
				}
				else if (this.type == Tonemapping.TonemapperType.Hable)
				{
					this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
					Graphics.Blit(source, destination, this.tonemapMaterial, 5);
				}
				else if (this.type == Tonemapping.TonemapperType.Photographic)
				{
					this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
					Graphics.Blit(source, destination, this.tonemapMaterial, 8);
				}
				else if (this.type == Tonemapping.TonemapperType.OptimizedHejiDawson)
				{
					this.tonemapMaterial.SetFloat("_ExposureAdjustment", 0.5f * this.exposureAdjustment);
					Graphics.Blit(source, destination, this.tonemapMaterial, 7);
				}
				else
				{
					bool flag = this.CreateInternalRenderTexture();
					RenderTexture temporary = RenderTexture.GetTemporary((int)this.adaptiveTextureSize, (int)this.adaptiveTextureSize, 0, this.rtFormat);
					Graphics.Blit(source, temporary);
					int num = (int)Mathf.Log((float)temporary.width * 1f, 2f);
					int num2 = 2;
					RenderTexture[] array = new RenderTexture[num];
					for (int i = 0; i < num; i++)
					{
						array[i] = RenderTexture.GetTemporary(temporary.width / num2, temporary.width / num2, 0, this.rtFormat);
						num2 *= 2;
					}
					RenderTexture source2 = array[num - 1];
					Graphics.Blit(temporary, array[0], this.tonemapMaterial, 1);
					if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
					{
						for (int j = 0; j < num - 1; j++)
						{
							Graphics.Blit(array[j], array[j + 1], this.tonemapMaterial, 9);
							source2 = array[j + 1];
						}
					}
					else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
					{
						for (int k = 0; k < num - 1; k++)
						{
							Graphics.Blit(array[k], array[k + 1]);
							source2 = array[k + 1];
						}
					}
					this.adaptionSpeed = ((this.adaptionSpeed >= 0.001f) ? this.adaptionSpeed : 0.001f);
					this.tonemapMaterial.SetFloat("_AdaptionSpeed", this.adaptionSpeed);
					this.rt.MarkRestoreExpected();
					Graphics.Blit(source2, this.rt, this.tonemapMaterial, (!flag) ? 2 : 3);
					this.middleGrey = ((this.middleGrey >= 0.001f) ? this.middleGrey : 0.001f);
					this.tonemapMaterial.SetVector("_HdrParams", new Vector4(this.middleGrey, this.middleGrey, this.middleGrey, this.white * this.white));
					this.tonemapMaterial.SetTexture("_SmallTex", this.rt);
					if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
					{
						Graphics.Blit(source, destination, this.tonemapMaterial, 0);
					}
					else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
					{
						Graphics.Blit(source, destination, this.tonemapMaterial, 10);
					}
					else
					{
						Debug.LogError("No valid adaptive tonemapper type found!");
						Graphics.Blit(source, destination);
					}
					for (int l = 0; l < num; l++)
					{
						RenderTexture.ReleaseTemporary(array[l]);
					}
					RenderTexture.ReleaseTemporary(temporary);
				}
			}
		}

		// Token: 0x04000879 RID: 2169
		public Tonemapping.TonemapperType type = Tonemapping.TonemapperType.Photographic;

		// Token: 0x0400087A RID: 2170
		public Tonemapping.AdaptiveTexSize adaptiveTextureSize = Tonemapping.AdaptiveTexSize.Square256;

		// Token: 0x0400087B RID: 2171
		public AnimationCurve remapCurve;

		// Token: 0x0400087C RID: 2172
		private Texture2D curveTex = null;

		// Token: 0x0400087D RID: 2173
		public float exposureAdjustment = 1.5f;

		// Token: 0x0400087E RID: 2174
		public float middleGrey = 0.4f;

		// Token: 0x0400087F RID: 2175
		public float white = 2f;

		// Token: 0x04000880 RID: 2176
		public float adaptionSpeed = 1.5f;

		// Token: 0x04000881 RID: 2177
		public Shader tonemapper = null;

		// Token: 0x04000882 RID: 2178
		public bool validRenderTextureFormat = true;

		// Token: 0x04000883 RID: 2179
		private Material tonemapMaterial = null;

		// Token: 0x04000884 RID: 2180
		private RenderTexture rt = null;

		// Token: 0x04000885 RID: 2181
		private RenderTextureFormat rtFormat = RenderTextureFormat.ARGBHalf;

		// Token: 0x020001B0 RID: 432
		public enum TonemapperType
		{
			// Token: 0x04000887 RID: 2183
			SimpleReinhard,
			// Token: 0x04000888 RID: 2184
			UserCurve,
			// Token: 0x04000889 RID: 2185
			Hable,
			// Token: 0x0400088A RID: 2186
			Photographic,
			// Token: 0x0400088B RID: 2187
			OptimizedHejiDawson,
			// Token: 0x0400088C RID: 2188
			AdaptiveReinhard,
			// Token: 0x0400088D RID: 2189
			AdaptiveReinhardAutoWhite
		}

		// Token: 0x020001B1 RID: 433
		public enum AdaptiveTexSize
		{
			// Token: 0x0400088F RID: 2191
			Square16 = 16,
			// Token: 0x04000890 RID: 2192
			Square32 = 32,
			// Token: 0x04000891 RID: 2193
			Square64 = 64,
			// Token: 0x04000892 RID: 2194
			Square128 = 128,
			// Token: 0x04000893 RID: 2195
			Square256 = 256,
			// Token: 0x04000894 RID: 2196
			Square512 = 512,
			// Token: 0x04000895 RID: 2197
			Square1024 = 1024
		}
	}
}
