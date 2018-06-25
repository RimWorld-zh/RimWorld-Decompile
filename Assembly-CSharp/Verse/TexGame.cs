using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020009CC RID: 2508
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		// Token: 0x040023FB RID: 9211
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		// Token: 0x040023FC RID: 9212
		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		// Token: 0x040023FD RID: 9213
		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);

		// Token: 0x0600383E RID: 14398 RVA: 0x001DFDF8 File Offset: 0x001DE1F8
		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
		}
	}
}
