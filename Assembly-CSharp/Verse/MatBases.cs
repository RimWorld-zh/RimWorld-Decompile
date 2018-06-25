using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F64 RID: 3940
	[StaticConstructorOnStartup]
	public static class MatBases
	{
		// Token: 0x04003E75 RID: 15989
		public static readonly Material LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay", -1);

		// Token: 0x04003E76 RID: 15990
		public static readonly Material SunShadow = MatLoader.LoadMat("Lighting/SunShadow", -1);

		// Token: 0x04003E77 RID: 15991
		public static readonly Material SunShadowFade = MatBases.SunShadow;

		// Token: 0x04003E78 RID: 15992
		public static readonly Material EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow", -1);

		// Token: 0x04003E79 RID: 15993
		public static readonly Material IndoorMask = MatLoader.LoadMat("Misc/IndoorMask", -1);

		// Token: 0x04003E7A RID: 15994
		public static readonly Material FogOfWar = MatLoader.LoadMat("Misc/FogOfWar", -1);

		// Token: 0x04003E7B RID: 15995
		public static readonly Material Snow = MatLoader.LoadMat("Misc/Snow", -1);
	}
}
