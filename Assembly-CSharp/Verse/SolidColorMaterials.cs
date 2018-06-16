using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F6C RID: 3948
	public static class SolidColorMaterials
	{
		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06005F3A RID: 24378 RVA: 0x00307FB8 File Offset: 0x003063B8
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x00307FE4 File Offset: 0x003063E4
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

		// Token: 0x06005F3C RID: 24380 RVA: 0x00308064 File Offset: 0x00306464
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

		// Token: 0x06005F3D RID: 24381 RVA: 0x003080D8 File Offset: 0x003064D8
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06005F3E RID: 24382 RVA: 0x003080FC File Offset: 0x003064FC
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

		// Token: 0x04003EAA RID: 16042
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x04003EAB RID: 16043
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();
	}
}
