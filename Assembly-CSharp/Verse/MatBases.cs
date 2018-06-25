using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F65 RID: 3941
	[StaticConstructorOnStartup]
	public static class MatBases
	{
		// Token: 0x04003E7D RID: 15997
		public static readonly Material LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay", -1);

		// Token: 0x04003E7E RID: 15998
		public static readonly Material SunShadow = MatLoader.LoadMat("Lighting/SunShadow", -1);

		// Token: 0x04003E7F RID: 15999
		public static readonly Material SunShadowFade = MatBases.SunShadow;

		// Token: 0x04003E80 RID: 16000
		public static readonly Material EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow", -1);

		// Token: 0x04003E81 RID: 16001
		public static readonly Material IndoorMask = MatLoader.LoadMat("Misc/IndoorMask", -1);

		// Token: 0x04003E82 RID: 16002
		public static readonly Material FogOfWar = MatLoader.LoadMat("Misc/FogOfWar", -1);

		// Token: 0x04003E83 RID: 16003
		public static readonly Material Snow = MatLoader.LoadMat("Misc/Snow", -1);
	}
}
