using System;

namespace RimWorld
{
	// Token: 0x0200091C RID: 2332
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

		// Token: 0x06003626 RID: 13862 RVA: 0x001D0A61 File Offset: 0x001CEE61
		static FactionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
		}
	}
}
