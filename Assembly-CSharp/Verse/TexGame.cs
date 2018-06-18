using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020009CE RID: 2510
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		// Token: 0x06003840 RID: 14400 RVA: 0x001DF808 File Offset: 0x001DDC08
		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
		}

		// Token: 0x040023F8 RID: 9208
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		// Token: 0x040023F9 RID: 9209
		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		// Token: 0x040023FA RID: 9210
		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);
	}
}
