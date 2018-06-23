using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096F RID: 2415
	[DefOf]
	public static class ShaderTypeDefOf
	{
		// Token: 0x0400231B RID: 8987
		public static ShaderTypeDef Cutout;

		// Token: 0x0400231C RID: 8988
		public static ShaderTypeDef CutoutComplex;

		// Token: 0x0400231D RID: 8989
		public static ShaderTypeDef Transparent;

		// Token: 0x0400231E RID: 8990
		public static ShaderTypeDef MetaOverlay;

		// Token: 0x0400231F RID: 8991
		public static ShaderTypeDef EdgeDetect;

		// Token: 0x06003677 RID: 13943 RVA: 0x001D0F1B File Offset: 0x001CF31B
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDef));
		}
	}
}
