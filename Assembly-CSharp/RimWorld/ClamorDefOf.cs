using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096D RID: 2413
	[DefOf]
	public static class ClamorDefOf
	{
		// Token: 0x04002318 RID: 8984
		public static ClamorDef Movement;

		// Token: 0x04002319 RID: 8985
		public static ClamorDef Harm;

		// Token: 0x0400231A RID: 8986
		public static ClamorDef Construction;

		// Token: 0x0400231B RID: 8987
		public static ClamorDef Impact;

		// Token: 0x06003677 RID: 13943 RVA: 0x001D12E7 File Offset: 0x001CF6E7
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}
	}
}
