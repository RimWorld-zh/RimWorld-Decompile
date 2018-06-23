using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F6B RID: 3947
	public static class SolidColorMaterials
	{
		// Token: 0x04003EBB RID: 16059
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x04003EBC RID: 16060
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06005F61 RID: 24417 RVA: 0x0030A138 File Offset: 0x00308538
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x06005F62 RID: 24418 RVA: 0x0030A164 File Offset: 0x00308564
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

		// Token: 0x06005F63 RID: 24419 RVA: 0x0030A1E4 File Offset: 0x003085E4
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

		// Token: 0x06005F64 RID: 24420 RVA: 0x0030A258 File Offset: 0x00308658
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06005F65 RID: 24421 RVA: 0x0030A27C File Offset: 0x0030867C
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
