using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F70 RID: 3952
	public static class SolidColorMaterials
	{
		// Token: 0x04003EC6 RID: 16070
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x04003EC7 RID: 16071
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06005F6B RID: 24427 RVA: 0x0030A9FC File Offset: 0x00308DFC
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x06005F6C RID: 24428 RVA: 0x0030AA28 File Offset: 0x00308E28
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

		// Token: 0x06005F6D RID: 24429 RVA: 0x0030AAA8 File Offset: 0x00308EA8
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

		// Token: 0x06005F6E RID: 24430 RVA: 0x0030AB1C File Offset: 0x00308F1C
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06005F6F RID: 24431 RVA: 0x0030AB40 File Offset: 0x00308F40
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
	}
}
