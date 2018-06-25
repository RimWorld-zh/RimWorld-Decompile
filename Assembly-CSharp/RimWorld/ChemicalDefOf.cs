using System;

namespace RimWorld
{
	// Token: 0x02000947 RID: 2375
	[DefOf]
	public static class ChemicalDefOf
	{
		// Token: 0x04002209 RID: 8713
		public static ChemicalDef Alcohol;

		// Token: 0x06003651 RID: 13905 RVA: 0x001D103B File Offset: 0x001CF43B
		static ChemicalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ChemicalDefOf));
		}
	}
}
