using System;

namespace RimWorld
{
	// Token: 0x0200091E RID: 2334
	[DefOf]
	public static class FactionDefOf
	{
		// Token: 0x06003627 RID: 13863 RVA: 0x001D0671 File Offset: 0x001CEA71
		static FactionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
		}

		// Token: 0x04001EC2 RID: 7874
		public static FactionDef PlayerColony;

		// Token: 0x04001EC3 RID: 7875
		public static FactionDef PlayerTribe;

		// Token: 0x04001EC4 RID: 7876
		public static FactionDef Ancients;

		// Token: 0x04001EC5 RID: 7877
		public static FactionDef AncientsHostile;

		// Token: 0x04001EC6 RID: 7878
		public static FactionDef Mechanoid;

		// Token: 0x04001EC7 RID: 7879
		public static FactionDef Insect;
	}
}
