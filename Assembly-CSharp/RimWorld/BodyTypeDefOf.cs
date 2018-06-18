using System;

namespace RimWorld
{
	// Token: 0x02000971 RID: 2417
	[DefOf]
	public static class BodyTypeDefOf
	{
		// Token: 0x0600367C RID: 13948 RVA: 0x001D0D0F File Offset: 0x001CF10F
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDef));
		}

		// Token: 0x04002317 RID: 8983
		public static BodyTypeDef Male;

		// Token: 0x04002318 RID: 8984
		public static BodyTypeDef Female;

		// Token: 0x04002319 RID: 8985
		public static BodyTypeDef Thin;

		// Token: 0x0400231A RID: 8986
		public static BodyTypeDef Hulk;

		// Token: 0x0400231B RID: 8987
		public static BodyTypeDef Fat;
	}
}
