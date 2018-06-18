using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F6B RID: 3947
	public static class SolidColorMaterials
	{
		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06005F38 RID: 24376 RVA: 0x00308094 File Offset: 0x00306494
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x06005F39 RID: 24377 RVA: 0x003080C0 File Offset: 0x003064C0
		public static Material SimpleSolidColorMaterial(Color col, bool careAboutVertexColors = false)
		{
			Material material;
			if (careAboutVertexColors)
			{
				if (!SolidColorMaterials.simpleColorAndVertexColorMats.TryGetValue(col, out material))
				{
					material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.VertexColor);
					SolidColorMaterials.simpleColorAndVertexColorMats.Add(col, material);
				}
			}
			else if (!SolidColorMaterials.simpleColorMats.TryGetValue(col, out material))
			{
				material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.SolidColor);
				SolidColorMaterials.simpleColorMats.Add(col, material);
			}
			return material;
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x00308140 File Offset: 0x00306540
		public static Material NewSolidColorMaterial(Color col, Shader shader)
		{
			Material result;
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a material from a different thread.", false);
				result = null;
			}
			else
			{
				Material material = MaterialAllocator.Create(shader);
				material.color = col;
				material.name = string.Concat(new object[]
				{
					"SolidColorMat-",
					shader.name,
					"-",
					col
				});
				result = material;
			}
			return result;
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x003081B4 File Offset: 0x003065B4
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x003081D8 File Offset: 0x003065D8
		public static Texture2D NewSolidColorTexture(Color color)
		{
			Texture2D result;
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a texture from a different thread.", false);
				result = null;
			}
			else
			{
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.name = "SolidColorTex-" + color;
				texture2D.SetPixel(0, 0, color);
				texture2D.Apply();
				result = texture2D;
			}
			return result;
		}

		// Token: 0x04003EA9 RID: 16041
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x04003EAA RID: 16042
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();
	}
}
