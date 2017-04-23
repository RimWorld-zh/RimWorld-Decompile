using System;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class ShaderPropertyIDs
	{
		private static readonly string SunLightDirectionName = "_PlanetSunLightDirection";

		private static readonly string SunLightEnabledName = "_PlanetSunLightEnabled";

		private static readonly string PlanetRadiusName = "_PlanetRadius";

		private static readonly string GlowRadiusName = "_GlowRadius";

		private static readonly string ColorName = "_Color";

		private static readonly string ColorTwoName = "_ColorTwo";

		private static readonly string MaskTexName = "_MaskTex";

		public static int SunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.SunLightDirectionName);

		public static int SunLightEnabled = Shader.PropertyToID(ShaderPropertyIDs.SunLightEnabledName);

		public static int PlanetRadius = Shader.PropertyToID(ShaderPropertyIDs.PlanetRadiusName);

		public static int GlowRadius = Shader.PropertyToID(ShaderPropertyIDs.GlowRadiusName);

		public static int Color = Shader.PropertyToID(ShaderPropertyIDs.ColorName);

		public static int ColorTwo = Shader.PropertyToID(ShaderPropertyIDs.ColorTwoName);

		public static int MaskTex = Shader.PropertyToID(ShaderPropertyIDs.MaskTexName);
	}
}
