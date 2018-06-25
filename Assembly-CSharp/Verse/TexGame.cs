using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020009CC RID: 2508
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		// Token: 0x040023F4 RID: 9204
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		// Token: 0x040023F5 RID: 9205
		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		// Token: 0x040023F6 RID: 9206
		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);

		// Token: 0x0600383E RID: 14398 RVA: 0x001DFB24 File Offset: 0x001DDF24
		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
		}
	}
}
