using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F66 RID: 3942
	[StaticConstructorOnStartup]
	public static class ShaderDatabase
	{
		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06005F30 RID: 24368 RVA: 0x00307ABC File Offset: 0x00305EBC
		public static Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x06005F31 RID: 24369 RVA: 0x00307AD8 File Offset: 0x00305ED8
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

		// Token: 0x04003E6A RID: 15978
		public static readonly Shader Cutout = ShaderDatabase.LoadShader("Map/Cutout");

		// Token: 0x04003E6B RID: 15979
		public static readonly Shader CutoutPlant = ShaderDatabase.LoadShader("Map/CutoutPlant");

		// Token: 0x04003E6C RID: 15980
		public static readonly Shader CutoutComplex = ShaderDatabase.LoadShader("Map/CutoutComplex");

		// Token: 0x04003E6D RID: 15981
		public static readonly Shader CutoutSkin = ShaderDatabase.LoadShader("Map/CutoutSkin");

		// Token: 0x04003E6E RID: 15982
		public static readonly Shader CutoutFlying = ShaderDatabase.LoadShader("Map/CutoutFlying");

		// Token: 0x04003E6F RID: 15983
		public static readonly Shader Transparent = ShaderDatabase.LoadShader("Map/Transparent");

		// Token: 0x04003E70 RID: 15984
		public static readonly Shader TransparentPostLight = ShaderDatabase.LoadShader("Map/TransparentPostLight");

		// Token: 0x04003E71 RID: 15985
		public static readonly Shader TransparentPlant = ShaderDatabase.LoadShader("Map/TransparentPlant");

		// Token: 0x04003E72 RID: 15986
		public static readonly Shader Mote = ShaderDatabase.LoadShader("Map/Mote");

		// Token: 0x04003E73 RID: 15987
		public static readonly Shader MoteGlow = ShaderDatabase.LoadShader("Map/MoteGlow");

		// Token: 0x04003E74 RID: 15988
		public static readonly Shader MoteWater = ShaderDatabase.LoadShader("Map/MoteWater");

		// Token: 0x04003E75 RID: 15989
		public static readonly Shader TerrainHard = ShaderDatabase.LoadShader("Map/TerrainHard");

		// Token: 0x04003E76 RID: 15990
		public static readonly Shader TerrainFade = ShaderDatabase.LoadShader("Map/TerrainFade");

		// Token: 0x04003E77 RID: 15991
		public static readonly Shader TerrainFadeRough = ShaderDatabase.LoadShader("Map/TerrainFadeRough");

		// Token: 0x04003E78 RID: 15992
		public static readonly Shader TerrainWater = ShaderDatabase.LoadShader("Map/TerrainWater");

		// Token: 0x04003E79 RID: 15993
		public static readonly Shader WorldTerrain = ShaderDatabase.LoadShader("World/WorldTerrain");

		// Token: 0x04003E7A RID: 15994
		public static readonly Shader WorldOcean = ShaderDatabase.LoadShader("World/WorldOcean");

		// Token: 0x04003E7B RID: 15995
		public static readonly Shader WorldOverlayCutout = ShaderDatabase.LoadShader("World/WorldOverlayCutout");

		// Token: 0x04003E7C RID: 15996
		public static readonly Shader WorldOverlayTransparent = ShaderDatabase.LoadShader("World/WorldOverlayTransparent");

		// Token: 0x04003E7D RID: 15997
		public static readonly Shader WorldOverlayTransparentLit = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLit");

		// Token: 0x04003E7E RID: 15998
		public static readonly Shader WorldOverlayAdditive = ShaderDatabase.LoadShader("World/WorldOverlayAdditive");

		// Token: 0x04003E7F RID: 15999
		public static readonly Shader MetaOverlay = ShaderDatabase.LoadShader("Map/MetaOverlay");

		// Token: 0x04003E80 RID: 16000
		public static readonly Shader SolidColor = ShaderDatabase.LoadShader("Map/SolidColor");

		// Token: 0x04003E81 RID: 16001
		public static readonly Shader VertexColor = ShaderDatabase.LoadShader("Map/VertexColor");

		// Token: 0x04003E82 RID: 16002
		private static Dictionary<string, Shader> lookup;
	}
}
