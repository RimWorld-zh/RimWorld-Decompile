using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096B RID: 2411
	[DefOf]
	public static class ClamorDefOf
	{
		// Token: 0x06003673 RID: 13939 RVA: 0x001D0ED3 File Offset: 0x001CF2D3
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}

		// Token: 0x04002310 RID: 8976
		public static ClamorDef Movement;

		// Token: 0x04002311 RID: 8977
		public static ClamorDef Harm;

		// Token: 0x04002312 RID: 8978
		public static ClamorDef Construction;

		// Token: 0x04002313 RID: 8979
		public static ClamorDef Impact;
	}
}
