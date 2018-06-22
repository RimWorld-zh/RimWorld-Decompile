using System;

namespace RimWorld
{
	// Token: 0x0200096D RID: 2413
	[DefOf]
	public static class BodyTypeDefOf
	{
		// Token: 0x06003675 RID: 13941 RVA: 0x001D0EF7 File Offset: 0x001CF2F7
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDef));
		}

		// Token: 0x04002315 RID: 8981
		public static BodyTypeDef Male;

		// Token: 0x04002316 RID: 8982
		public static BodyTypeDef Female;

		// Token: 0x04002317 RID: 8983
		public static BodyTypeDef Thin;

		// Token: 0x04002318 RID: 8984
		public static BodyTypeDef Hulk;

		// Token: 0x04002319 RID: 8985
		public static BodyTypeDef Fat;
	}
}
