using System;

namespace RimWorld
{
	// Token: 0x02000945 RID: 2373
	[DefOf]
	public static class ChemicalDefOf
	{
		// Token: 0x04002201 RID: 8705
		public static ChemicalDef Alcohol;

		// Token: 0x0600364D RID: 13901 RVA: 0x001D0C27 File Offset: 0x001CF027
		static ChemicalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ChemicalDefOf));
		}
	}
}
