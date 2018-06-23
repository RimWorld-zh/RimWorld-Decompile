using System;

namespace RimWorld
{
	// Token: 0x02000936 RID: 2358
	[DefOf]
	public static class StatCategoryDefOf
	{
		// Token: 0x04002053 RID: 8275
		public static StatCategoryDef Basics;

		// Token: 0x04002054 RID: 8276
		public static StatCategoryDef BasicsNonPawn;

		// Token: 0x04002055 RID: 8277
		public static StatCategoryDef BasicsPawn;

		// Token: 0x04002056 RID: 8278
		public static StatCategoryDef Apparel;

		// Token: 0x04002057 RID: 8279
		public static StatCategoryDef Weapon;

		// Token: 0x04002058 RID: 8280
		public static StatCategoryDef Building;

		// Token: 0x04002059 RID: 8281
		public static StatCategoryDef PawnWork;

		// Token: 0x0400205A RID: 8282
		public static StatCategoryDef PawnCombat;

		// Token: 0x0400205B RID: 8283
		public static StatCategoryDef PawnSocial;

		// Token: 0x0400205C RID: 8284
		public static StatCategoryDef PawnMisc;

		// Token: 0x0400205D RID: 8285
		public static StatCategoryDef EquippedStatOffsets;

		// Token: 0x0400205E RID: 8286
		public static StatCategoryDef StuffStatFactors;

		// Token: 0x0400205F RID: 8287
		public static StatCategoryDef StuffStatOffsets;

		// Token: 0x04002060 RID: 8288
		public static StatCategoryDef StuffOfEquipmentStatFactors;

		// Token: 0x04002061 RID: 8289
		public static StatCategoryDef Surgery;

		// Token: 0x0600363E RID: 13886 RVA: 0x001D0B19 File Offset: 0x001CEF19
		static StatCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatCategoryDefOf));
		}
	}
}
