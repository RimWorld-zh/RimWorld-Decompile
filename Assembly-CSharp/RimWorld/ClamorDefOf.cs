using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096D RID: 2413
	[DefOf]
	public static class ClamorDefOf
	{
		// Token: 0x04002311 RID: 8977
		public static ClamorDef Movement;

		// Token: 0x04002312 RID: 8978
		public static ClamorDef Harm;

		// Token: 0x04002313 RID: 8979
		public static ClamorDef Construction;

		// Token: 0x04002314 RID: 8980
		public static ClamorDef Impact;

		// Token: 0x06003677 RID: 13943 RVA: 0x001D1013 File Offset: 0x001CF413
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}
	}
}
