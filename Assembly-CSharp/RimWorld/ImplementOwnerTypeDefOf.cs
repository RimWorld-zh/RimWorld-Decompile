using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000965 RID: 2405
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		// Token: 0x040022D8 RID: 8920
		public static ImplementOwnerTypeDef Weapon;

		// Token: 0x040022D9 RID: 8921
		public static ImplementOwnerTypeDef Bodypart;

		// Token: 0x040022DA RID: 8922
		public static ImplementOwnerTypeDef Hediff;

		// Token: 0x040022DB RID: 8923
		public static ImplementOwnerTypeDef Terrain;

		// Token: 0x0600366F RID: 13935 RVA: 0x001D0F83 File Offset: 0x001CF383
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}
	}
}
