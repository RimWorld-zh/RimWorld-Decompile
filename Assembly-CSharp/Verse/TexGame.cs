using System;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);

		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
		}
	}
}
