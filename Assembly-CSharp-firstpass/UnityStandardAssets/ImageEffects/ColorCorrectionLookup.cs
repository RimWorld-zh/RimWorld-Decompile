using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000189 RID: 393
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (3D Lookup Texture)")]
	public class ColorCorrectionLookup : PostEffectsBase
	{
		// Token: 0x060008E3 RID: 2275 RVA: 0x000132E0 File Offset: 0x000114E0
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.material = base.CheckShaderAndCreateMaterial(this.shader, this.material);
			if (!this.isSupported || !SystemInfo.supports3DTextures)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00013336 File Offset: 0x00011536
		private void OnDisable()
		{
			if (this.material)
			{
				UnityEngine.Object.DestroyImmediate(this.material);
				this.material = null;
			}
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001335D File Offset: 0x0001155D
		private void OnDestroy()
		{
			if (this.converted3DLut)
			{
				UnityEngine.Object.DestroyImmediate(this.converted3DLut);
			}
			this.converted3DLut = null;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00013384 File Offset: 0x00011584
		public void SetIdentityLut()
		{
			int num = 16;
			Color[] array = new Color[num * num * num];
			float num2 = 1f / (1f * (float)num - 1f);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						array[i + j * num + k * num * num] = new Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
					}
				}
			}
			if (this.converted3DLut)
			{
				UnityEngine.Object.DestroyImmediate(this.converted3DLut);
			}
			this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
			this.converted3DLut.SetPixels(array);
			this.converted3DLut.Apply();
			this.basedOnTempTex = "";
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00013488 File Offset: 0x00011688
		public bool ValidDimensions(Texture2D tex2d)
		{
			bool result;
			if (!tex2d)
			{
				result = false;
			}
			else
			{
				int height = tex2d.height;
				result = (height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2d.width)));
			}
			return result;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x000134D8 File Offset: 0x000116D8
		public void Convert(Texture2D temp2DTex, string path)
		{
			if (temp2DTex)
			{
				int num = temp2DTex.width * temp2DTex.height;
				num = temp2DTex.height;
				if (!this.ValidDimensions(temp2DTex))
				{
					Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
					this.basedOnTempTex = "";
				}
				else
				{
					Color[] pixels = temp2DTex.GetPixels();
					Color[] array = new Color[pixels.Length];
					for (int i = 0; i < num; i++)
					{
						for (int j = 0; j < num; j++)
						{
							for (int k = 0; k < num; k++)
							{
								int num2 = num - j - 1;
								array[i + j * num + k * num * num] = pixels[k * num + i + num2 * num * num];
							}
						}
					}
					if (this.converted3DLut)
					{
						UnityEngine.Object.DestroyImmediate(this.converted3DLut);
					}
					this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
					this.converted3DLut.SetPixels(array);
					this.converted3DLut.Apply();
					this.basedOnTempTex = path;
				}
			}
			else
			{
				Debug.LogError("Couldn't color correct with 3D LUT texture. Image Effect will be disabled.");
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00013628 File Offset: 0x00011828
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || !SystemInfo.supports3DTextures)
			{
				Graphics.Blit(source, destination);
			}
			else
			{
				if (this.converted3DLut == null)
				{
					this.SetIdentityLut();
				}
				int width = this.converted3DLut.width;
				this.converted3DLut.wrapMode = TextureWrapMode.Clamp;
				this.material.SetFloat("_Scale", (float)(width - 1) / (1f * (float)width));
				this.material.SetFloat("_Offset", 1f / (2f * (float)width));
				this.material.SetTexture("_ClutTex", this.converted3DLut);
				Graphics.Blit(source, destination, this.material, (QualitySettings.activeColorSpace != ColorSpace.Linear) ? 0 : 1);
			}
		}

		// Token: 0x0400076A RID: 1898
		public Shader shader;

		// Token: 0x0400076B RID: 1899
		private Material material;

		// Token: 0x0400076C RID: 1900
		public Texture3D converted3DLut = null;

		// Token: 0x0400076D RID: 1901
		public string basedOnTempTex = "";
	}
}
