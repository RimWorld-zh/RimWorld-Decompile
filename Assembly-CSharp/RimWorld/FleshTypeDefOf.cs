using System;

namespace RimWorld
{
	// Token: 0x02000954 RID: 2388
	[DefOf]
	public static class FleshTypeDefOf
	{
		// Token: 0x04002297 RID: 8855
		public static FleshTypeDef Normal;

		// Token: 0x04002298 RID: 8856
		public static FleshTypeDef Mechanoid;

		// Token: 0x04002299 RID: 8857
		public static FleshTypeDef Insectoid;

		// Token: 0x0600365C RID: 13916 RVA: 0x001D0D35 File Offset: 0x001CF135
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}
	}
}
