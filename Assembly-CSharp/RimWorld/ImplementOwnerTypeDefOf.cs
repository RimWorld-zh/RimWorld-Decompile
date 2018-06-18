using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000967 RID: 2407
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		// Token: 0x06003672 RID: 13938 RVA: 0x001D0C5B File Offset: 0x001CF05B
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}

		// Token: 0x040022D9 RID: 8921
		public static ImplementOwnerTypeDef Weapon;

		// Token: 0x040022DA RID: 8922
		public static ImplementOwnerTypeDef Bodypart;

		// Token: 0x040022DB RID: 8923
		public static ImplementOwnerTypeDef Hediff;

		// Token: 0x040022DC RID: 8924
		public static ImplementOwnerTypeDef Terrain;
	}
}
