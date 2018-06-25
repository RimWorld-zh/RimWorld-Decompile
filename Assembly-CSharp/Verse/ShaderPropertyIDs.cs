using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F6D RID: 3949
	[StaticConstructorOnStartup]
	public static class ShaderPropertyIDs
	{
		// Token: 0x04003EA0 RID: 16032
		private static readonly string PlanetSunLightDirectionName = "_PlanetSunLightDirection";

		// Token: 0x04003EA1 RID: 16033
		private static readonly string PlanetSunLightEnabledName = "_PlanetSunLightEnabled";

		// Token: 0x04003EA2 RID: 16034
		private static readonly string PlanetRadiusName = "_PlanetRadius";

		// Token: 0x04003EA3 RID: 16035
		private static readonly string MapSunLightDirectionName = "_CastVect";

		// Token: 0x04003EA4 RID: 16036
		private static readonly string GlowRadiusName = "_GlowRadius";

		// Token: 0x04003EA5 RID: 16037
		private static readonly string GameSecondsName = "_GameSeconds";

		// Token: 0x04003EA6 RID: 16038
		private static readonly string ColorName = "_Color";

		// Token: 0x04003EA7 RID: 16039
		private static readonly string ColorTwoName = "_ColorTwo";

		// Token: 0x04003EA8 RID: 16040
		private static readonly string MaskTexName = "_MaskTex";

		// Token: 0x04003EA9 RID: 16041
		private static readonly string SwayHeadName = "_SwayHead";

		// Token: 0x04003EAA RID: 16042
		private static readonly string ShockwaveSpanName = "_ShockwaveSpan";

		// Token: 0x04003EAB RID: 16043
		public static int PlanetSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightDirectionName);

		// Token: 0x04003EAC RID: 16044
		public static int PlanetSunLightEnabled = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightEnabledName);

		// Token: 0x04003EAD RID: 16045
		public static int PlanetRadius = Shader.PropertyToID(ShaderPropertyIDs.PlanetRadiusName);

		// Token: 0x04003EAE RID: 16046
		public static int MapSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.MapSunLightDirectionName);

		// Token: 0x04003EAF RID: 16047
		public static int GlowRadius = Shader.PropertyToID(ShaderPropertyIDs.GlowRadiusName);

		// Token: 0x04003EB0 RID: 16048
		public static int GameSeconds = Shader.PropertyToID(ShaderPropertyIDs.GameSecondsName);

		// Token: 0x04003EB1 RID: 16049
		public static int Color = Shader.PropertyToID(ShaderPropertyIDs.ColorName);

		// Token: 0x04003EB2 RID: 16050
		public static int ColorTwo = Shader.PropertyToID(ShaderPropertyIDs.ColorTwoName);

		// Token: 0x04003EB3 RID: 16051
		public static int MaskTex = Shader.PropertyToID(ShaderPropertyIDs.MaskTexName);

		// Token: 0x04003EB4 RID: 16052
		public static int SwayHead = Shader.PropertyToID(ShaderPropertyIDs.SwayHeadName);

		// Token: 0x04003EB5 RID: 16053
		public static int ShockwaveColor = Shader.PropertyToID("_ShockwaveColor");

		// Token: 0x04003EB6 RID: 16054
		public static int ShockwaveSpan = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveSpanName);

		// Token: 0x04003EB7 RID: 16055
		public static int WaterCastVectSun = Shader.PropertyToID("_WaterCastVectSun");

		// Token: 0x04003EB8 RID: 16056
		public static int WaterCastVectMoon = Shader.PropertyToID("_WaterCastVectMoon");

		// Token: 0x04003EB9 RID: 16057
		public static int WaterOutputTex = Shader.PropertyToID("_WaterOutputTex");

		// Token: 0x04003EBA RID: 16058
		public static int WaterOffsetTex = Shader.PropertyToID("_WaterOffsetTex");

		// Token: 0x04003EBB RID: 16059
		public static int ShadowCompositeTex = Shader.PropertyToID("_ShadowCompositeTex");

		// Token: 0x04003EBC RID: 16060
		public static int FallIntensity = Shader.PropertyToID("_FallIntensity");
	}
}
