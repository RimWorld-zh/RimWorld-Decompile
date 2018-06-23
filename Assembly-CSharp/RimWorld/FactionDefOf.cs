using System;

namespace RimWorld
{
	// Token: 0x0200091A RID: 2330
	[DefOf]
	public static class FactionDefOf
	{
		// Token: 0x04001EC0 RID: 7872
		public static FactionDef PlayerColony;

		// Token: 0x04001EC1 RID: 7873
		public static FactionDef PlayerTribe;

		// Token: 0x04001EC2 RID: 7874
		public static FactionDef Ancients;

		// Token: 0x04001EC3 RID: 7875
		public static FactionDef AncientsHostile;

		// Token: 0x04001EC4 RID: 7876
		public static FactionDef Mechanoid;

		// Token: 0x04001EC5 RID: 7877
		public static FactionDef Insect;

		// Token: 0x06003622 RID: 13858 RVA: 0x001D0921 File Offset: 0x001CED21
		static FactionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
		}
	}
}
