using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000963 RID: 2403
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		// Token: 0x0600366B RID: 13931 RVA: 0x001D0E43 File Offset: 0x001CF243
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}

		// Token: 0x040022D7 RID: 8919
		public static ImplementOwnerTypeDef Weapon;

		// Token: 0x040022D8 RID: 8920
		public static ImplementOwnerTypeDef Bodypart;

		// Token: 0x040022D9 RID: 8921
		public static ImplementOwnerTypeDef Hediff;

		// Token: 0x040022DA RID: 8922
		public static ImplementOwnerTypeDef Terrain;
	}
}
