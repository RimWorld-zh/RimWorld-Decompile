using System;

namespace RimWorld
{
	// Token: 0x0200096F RID: 2415
	[DefOf]
	public static class BodyTypeDefOf
	{
		// Token: 0x04002316 RID: 8982
		public static BodyTypeDef Male;

		// Token: 0x04002317 RID: 8983
		public static BodyTypeDef Female;

		// Token: 0x04002318 RID: 8984
		public static BodyTypeDef Thin;

		// Token: 0x04002319 RID: 8985
		public static BodyTypeDef Hulk;

		// Token: 0x0400231A RID: 8986
		public static BodyTypeDef Fat;

		// Token: 0x06003679 RID: 13945 RVA: 0x001D1037 File Offset: 0x001CF437
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDef));
		}
	}
}
