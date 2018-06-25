using System;

namespace RimWorld
{
	// Token: 0x02000947 RID: 2375
	[DefOf]
	public static class ChemicalDefOf
	{
		// Token: 0x04002202 RID: 8706
		public static ChemicalDef Alcohol;

		// Token: 0x06003651 RID: 13905 RVA: 0x001D0D67 File Offset: 0x001CF167
		static ChemicalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ChemicalDefOf));
		}
	}
}
