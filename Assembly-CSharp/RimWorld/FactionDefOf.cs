using System;

namespace RimWorld
{
	// Token: 0x0200091C RID: 2332
	[DefOf]
	public static class FactionDefOf
	{
		// Token: 0x04001EC7 RID: 7879
		public static FactionDef PlayerColony;

		// Token: 0x04001EC8 RID: 7880
		public static FactionDef PlayerTribe;

		// Token: 0x04001EC9 RID: 7881
		public static FactionDef Ancients;

		// Token: 0x04001ECA RID: 7882
		public static FactionDef AncientsHostile;

		// Token: 0x04001ECB RID: 7883
		public static FactionDef Mechanoid;

		// Token: 0x04001ECC RID: 7884
		public static FactionDef Insect;

		// Token: 0x06003626 RID: 13862 RVA: 0x001D0D35 File Offset: 0x001CF135
		static FactionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
		}
	}
}
