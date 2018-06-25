using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000971 RID: 2417
	[DefOf]
	public static class ShaderTypeDefOf
	{
		// Token: 0x04002323 RID: 8995
		public static ShaderTypeDef Cutout;

		// Token: 0x04002324 RID: 8996
		public static ShaderTypeDef CutoutComplex;

		// Token: 0x04002325 RID: 8997
		public static ShaderTypeDef Transparent;

		// Token: 0x04002326 RID: 8998
		public static ShaderTypeDef MetaOverlay;

		// Token: 0x04002327 RID: 8999
		public static ShaderTypeDef EdgeDetect;

		// Token: 0x0600367B RID: 13947 RVA: 0x001D132F File Offset: 0x001CF72F
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDef));
		}
	}
}
