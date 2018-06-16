using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F69 RID: 3945
	[StaticConstructorOnStartup]
	public static class ShaderPropertyIDs
	{
		// Token: 0x04003E84 RID: 16004
		private static readonly string PlanetSunLightDirectionName = "_PlanetSunLightDirection";

		// Token: 0x04003E85 RID: 16005
		private static readonly string PlanetSunLightEnabledName = "_PlanetSunLightEnabled";

		// Token: 0x04003E86 RID: 16006
		private static readonly string PlanetRadiusName = "_PlanetRadius";

		// Token: 0x04003E87 RID: 16007
		private static readonly string MapSunLightDirectionName = "_CastVect";

		// Token: 0x04003E88 RID: 16008
		private static readonly string GlowRadiusName = "_GlowRadius";

		// Token: 0x04003E89 RID: 16009
		private static readonly string GameSecondsName = "_GameSeconds";

		// Token: 0x04003E8A RID: 16010
		private static readonly string ColorName = "_Color";

		// Token: 0x04003E8B RID: 16011
		private static readonly string ColorTwoName = "_ColorTwo";

		// Token: 0x04003E8C RID: 16012
		private static readonly string MaskTexName = "_MaskTex";

		// Token: 0x04003E8D RID: 16013
		private static readonly string SwayHeadName = "_SwayHead";

		// Token: 0x04003E8E RID: 16014
		private static readonly string ShockwaveSpanName = "_ShockwaveSpan";

		// Token: 0x04003E8F RID: 16015
		public static int PlanetSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightDirectionName);

		// Token: 0x04003E90 RID: 16016
		public static int PlanetSunLightEnabled = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightEnabledName);

		// Token: 0x04003E91 RID: 16017
		public static int PlanetRadius = Shader.PropertyToID(ShaderPropertyIDs.PlanetRadiusName);

		// Token: 0x04003E92 RID: 16018
		public static int MapSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.MapSunLightDirectionName);

		// Token: 0x04003E93 RID: 16019
		public static int GlowRadius = Shader.PropertyToID(ShaderPropertyIDs.GlowRadiusName);

		// Token: 0x04003E94 RID: 16020
		public static int GameSeconds = Shader.PropertyToID(ShaderPropertyIDs.GameSecondsName);

		// Token: 0x04003E95 RID: 16021
		public static int Color = Shader.PropertyToID(ShaderPropertyIDs.ColorName);

		// Token: 0x04003E96 RID: 16022
		public static int ColorTwo = Shader.PropertyToID(ShaderPropertyIDs.ColorTwoName);

		// Token: 0x04003E97 RID: 16023
		public static int MaskTex = Shader.PropertyToID(ShaderPropertyIDs.MaskTexName);

		// Token: 0x04003E98 RID: 16024
		public static int SwayHead = Shader.PropertyToID(ShaderPropertyIDs.SwayHeadName);

		// Token: 0x04003E99 RID: 16025
		public static int ShockwaveColor = Shader.PropertyToID("_ShockwaveColor");

		// Token: 0x04003E9A RID: 16026
		public static int ShockwaveSpan = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveSpanName);

		// Token: 0x04003E9B RID: 16027
		public static int WaterCastVectSun = Shader.PropertyToID("_WaterCastVectSun");

		// Token: 0x04003E9C RID: 16028
		public static int WaterCastVectMoon = Shader.PropertyToID("_WaterCastVectMoon");

		// Token: 0x04003E9D RID: 16029
		public static int WaterOutputTex = Shader.PropertyToID("_WaterOutputTex");

		// Token: 0x04003E9E RID: 16030
		public static int WaterOffsetTex = Shader.PropertyToID("_WaterOffsetTex");

		// Token: 0x04003E9F RID: 16031
		public static int ShadowCompositeTex = Shader.PropertyToID("_ShadowCompositeTex");

		// Token: 0x04003EA0 RID: 16032
		public static int FallIntensity = Shader.PropertyToID("_FallIntensity");
	}
}
