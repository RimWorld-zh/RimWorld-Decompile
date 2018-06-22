using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020009CA RID: 2506
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		// Token: 0x0600383A RID: 14394 RVA: 0x001DF9E0 File Offset: 0x001DDDE0
		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
		}

		// Token: 0x040023F3 RID: 9203
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		// Token: 0x040023F4 RID: 9204
		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		// Token: 0x040023F5 RID: 9205
		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);
	}
}
