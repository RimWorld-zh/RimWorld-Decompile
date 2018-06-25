using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000971 RID: 2417
	[DefOf]
	public static class ShaderTypeDefOf
	{
		// Token: 0x0400231C RID: 8988
		public static ShaderTypeDef Cutout;

		// Token: 0x0400231D RID: 8989
		public static ShaderTypeDef CutoutComplex;

		// Token: 0x0400231E RID: 8990
		public static ShaderTypeDef Transparent;

		// Token: 0x0400231F RID: 8991
		public static ShaderTypeDef MetaOverlay;

		// Token: 0x04002320 RID: 8992
		public static ShaderTypeDef EdgeDetect;

		// Token: 0x0600367B RID: 13947 RVA: 0x001D105B File Offset: 0x001CF45B
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDef));
		}
	}
}
