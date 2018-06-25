using System;

namespace RimWorld
{
	// Token: 0x02000938 RID: 2360
	[DefOf]
	public static class StatCategoryDefOf
	{
		// Token: 0x0400205A RID: 8282
		public static StatCategoryDef Basics;

		// Token: 0x0400205B RID: 8283
		public static StatCategoryDef BasicsNonPawn;

		// Token: 0x0400205C RID: 8284
		public static StatCategoryDef BasicsPawn;

		// Token: 0x0400205D RID: 8285
		public static StatCategoryDef Apparel;

		// Token: 0x0400205E RID: 8286
		public static StatCategoryDef Weapon;

		// Token: 0x0400205F RID: 8287
		public static StatCategoryDef Building;

		// Token: 0x04002060 RID: 8288
		public static StatCategoryDef PawnWork;

		// Token: 0x04002061 RID: 8289
		public static StatCategoryDef PawnCombat;

		// Token: 0x04002062 RID: 8290
		public static StatCategoryDef PawnSocial;

		// Token: 0x04002063 RID: 8291
		public static StatCategoryDef PawnMisc;

		// Token: 0x04002064 RID: 8292
		public static StatCategoryDef EquippedStatOffsets;

		// Token: 0x04002065 RID: 8293
		public static StatCategoryDef StuffStatFactors;

		// Token: 0x04002066 RID: 8294
		public static StatCategoryDef StuffStatOffsets;

		// Token: 0x04002067 RID: 8295
		public static StatCategoryDef StuffOfEquipmentStatFactors;

		// Token: 0x04002068 RID: 8296
		public static StatCategoryDef Surgery;

		// Token: 0x06003642 RID: 13890 RVA: 0x001D0F2D File Offset: 0x001CF32D
		static StatCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatCategoryDefOf));
		}
	}
}
