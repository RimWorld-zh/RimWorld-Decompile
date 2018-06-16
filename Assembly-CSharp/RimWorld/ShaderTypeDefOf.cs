using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000973 RID: 2419
	[DefOf]
	public static class ShaderTypeDefOf
	{
		// Token: 0x0600367C RID: 13948 RVA: 0x001D0C6B File Offset: 0x001CF06B
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDef));
		}

		// Token: 0x0400231D RID: 8989
		public static ShaderTypeDef Cutout;

		// Token: 0x0400231E RID: 8990
		public static ShaderTypeDef CutoutComplex;

		// Token: 0x0400231F RID: 8991
		public static ShaderTypeDef Transparent;

		// Token: 0x04002320 RID: 8992
		public static ShaderTypeDef MetaOverlay;

		// Token: 0x04002321 RID: 8993
		public static ShaderTypeDef EdgeDetect;
	}
}
