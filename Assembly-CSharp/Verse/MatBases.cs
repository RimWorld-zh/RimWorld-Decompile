using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F60 RID: 3936
	[StaticConstructorOnStartup]
	public static class MatBases
	{
		// Token: 0x04003E60 RID: 15968
		public static readonly Material LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay", -1);

		// Token: 0x04003E61 RID: 15969
		public static readonly Material SunShadow = MatLoader.LoadMat("Lighting/SunShadow", -1);

		// Token: 0x04003E62 RID: 15970
		public static readonly Material SunShadowFade = MatBases.SunShadow;

		// Token: 0x04003E63 RID: 15971
		public static readonly Material EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow", -1);

		// Token: 0x04003E64 RID: 15972
		public static readonly Material IndoorMask = MatLoader.LoadMat("Misc/IndoorMask", -1);

		// Token: 0x04003E65 RID: 15973
		public static readonly Material FogOfWar = MatLoader.LoadMat("Misc/FogOfWar", -1);

		// Token: 0x04003E66 RID: 15974
		public static readonly Material Snow = MatLoader.LoadMat("Misc/Snow", -1);
	}
}
