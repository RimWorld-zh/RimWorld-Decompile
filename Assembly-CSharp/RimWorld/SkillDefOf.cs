using System;

namespace RimWorld
{
	// Token: 0x0200091B RID: 2331
	[DefOf]
	public static class SkillDefOf
	{
		// Token: 0x04001EBB RID: 7867
		public static SkillDef Construction;

		// Token: 0x04001EBC RID: 7868
		public static SkillDef Plants;

		// Token: 0x04001EBD RID: 7869
		public static SkillDef Intellectual;

		// Token: 0x04001EBE RID: 7870
		public static SkillDef Mining;

		// Token: 0x04001EBF RID: 7871
		public static SkillDef Shooting;

		// Token: 0x04001EC0 RID: 7872
		public static SkillDef Melee;

		// Token: 0x04001EC1 RID: 7873
		public static SkillDef Social;

		// Token: 0x04001EC2 RID: 7874
		public static SkillDef Animals;

		// Token: 0x04001EC3 RID: 7875
		public static SkillDef Cooking;

		// Token: 0x04001EC4 RID: 7876
		public static SkillDef Medicine;

		// Token: 0x04001EC5 RID: 7877
		public static SkillDef Artistic;

		// Token: 0x04001EC6 RID: 7878
		public static SkillDef Crafting;

		// Token: 0x06003625 RID: 13861 RVA: 0x001D0D23 File Offset: 0x001CF123
		static SkillDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SkillDefOf));
		}
	}
}
