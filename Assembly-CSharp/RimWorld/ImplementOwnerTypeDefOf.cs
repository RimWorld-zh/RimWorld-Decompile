using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000965 RID: 2405
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		// Token: 0x040022DF RID: 8927
		public static ImplementOwnerTypeDef Weapon;

		// Token: 0x040022E0 RID: 8928
		public static ImplementOwnerTypeDef Bodypart;

		// Token: 0x040022E1 RID: 8929
		public static ImplementOwnerTypeDef Hediff;

		// Token: 0x040022E2 RID: 8930
		public static ImplementOwnerTypeDef Terrain;

		// Token: 0x0600366F RID: 13935 RVA: 0x001D1257 File Offset: 0x001CF657
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}
	}
}
