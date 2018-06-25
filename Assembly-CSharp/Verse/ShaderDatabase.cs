using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F6B RID: 3947
	[StaticConstructorOnStartup]
	public static class ShaderDatabase
	{
		// Token: 0x04003E87 RID: 16007
		public static readonly Shader Cutout = ShaderDatabase.LoadShader("Map/Cutout");

		// Token: 0x04003E88 RID: 16008
		public static readonly Shader CutoutPlant = ShaderDatabase.LoadShader("Map/CutoutPlant");

		// Token: 0x04003E89 RID: 16009
		public static readonly Shader CutoutComplex = ShaderDatabase.LoadShader("Map/CutoutComplex");

		// Token: 0x04003E8A RID: 16010
		public static readonly Shader CutoutSkin = ShaderDatabase.LoadShader("Map/CutoutSkin");

		// Token: 0x04003E8B RID: 16011
		public static readonly Shader CutoutFlying = ShaderDatabase.LoadShader("Map/CutoutFlying");

		// Token: 0x04003E8C RID: 16012
		public static readonly Shader Transparent = ShaderDatabase.LoadShader("Map/Transparent");

		// Token: 0x04003E8D RID: 16013
		public static readonly Shader TransparentPostLight = ShaderDatabase.LoadShader("Map/TransparentPostLight");

		// Token: 0x04003E8E RID: 16014
		public static readonly Shader TransparentPlant = ShaderDatabase.LoadShader("Map/TransparentPlant");

		// Token: 0x04003E8F RID: 16015
		public static readonly Shader Mote = ShaderDatabase.LoadShader("Map/Mote");

		// Token: 0x04003E90 RID: 16016
		public static readonly Shader MoteGlow = ShaderDatabase.LoadShader("Map/MoteGlow");

		// Token: 0x04003E91 RID: 16017
		public static readonly Shader MoteWater = ShaderDatabase.LoadShader("Map/MoteWater");

		// Token: 0x04003E92 RID: 16018
		public static readonly Shader TerrainHard = ShaderDatabase.LoadShader("Map/TerrainHard");

		// Token: 0x04003E93 RID: 16019
		public static readonly Shader TerrainFade = ShaderDatabase.LoadShader("Map/TerrainFade");

		// Token: 0x04003E94 RID: 16020
		public static readonly Shader TerrainFadeRough = ShaderDatabase.LoadShader("Map/TerrainFadeRough");

		// Token: 0x04003E95 RID: 16021
		public static readonly Shader TerrainWater = ShaderDatabase.LoadShader("Map/TerrainWater");

		// Token: 0x04003E96 RID: 16022
		public static readonly Shader WorldTerrain = ShaderDatabase.LoadShader("World/WorldTerrain");

		// Token: 0x04003E97 RID: 16023
		public static readonly Shader WorldOcean = ShaderDatabase.LoadShader("World/WorldOcean");

		// Token: 0x04003E98 RID: 16024
		public static readonly Shader WorldOverlayCutout = ShaderDatabase.LoadShader("World/WorldOverlayCutout");

		// Token: 0x04003E99 RID: 16025
		public static readonly Shader WorldOverlayTransparent = ShaderDatabase.LoadShader("World/WorldOverlayTransparent");

		// Token: 0x04003E9A RID: 16026
		public static readonly Shader WorldOverlayTransparentLit = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLit");

		// Token: 0x04003E9B RID: 16027
		public static readonly Shader WorldOverlayAdditive = ShaderDatabase.LoadShader("World/WorldOverlayAdditive");

		// Token: 0x04003E9C RID: 16028
		public static readonly Shader MetaOverlay = ShaderDatabase.LoadShader("Map/MetaOverlay");

		// Token: 0x04003E9D RID: 16029
		public static readonly Shader SolidColor = ShaderDatabase.LoadShader("Map/SolidColor");

		// Token: 0x04003E9E RID: 16030
		public static readonly Shader VertexColor = ShaderDatabase.LoadShader("Map/VertexColor");

		// Token: 0x04003E9F RID: 16031
		private static Dictionary<string, Shader> lookup;

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06005F63 RID: 24419 RVA: 0x0030A424 File Offset: 0x00308824
		public static Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x06005F64 RID: 24420 RVA: 0x0030A440 File Offset: 0x00308840
		public static Shader LoadShader(string shaderPath)
		{
			if (ShaderDatabase.lookup == null)
			{
				ShaderDatabase.lookup = new Dictionary<string, Shader>();
			}
			if (!ShaderDatabase.lookup.ContainsKey(shaderPath))
			{
				ShaderDatabase.lookup[shaderPath] = (Shader)Resources.Load("Materials/" + shaderPath, typeof(Shader));
			}
			Shader shader = ShaderDatabase.lookup[shaderPath];
			Shader result;
			if (shader == null)
			{
				Log.Warning("Could not load shader " + shaderPath, false);
				result = ShaderDatabase.DefaultShader;
			}
			else
			{
				result = shader;
			}
			return result;
		}
	}
}
