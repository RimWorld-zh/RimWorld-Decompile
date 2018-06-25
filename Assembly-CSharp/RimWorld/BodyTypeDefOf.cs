using System;

namespace RimWorld
{
	// Token: 0x0200096F RID: 2415
	[DefOf]
	public static class BodyTypeDefOf
	{
		// Token: 0x0400231D RID: 8989
		public static BodyTypeDef Male;

		// Token: 0x0400231E RID: 8990
		public static BodyTypeDef Female;

		// Token: 0x0400231F RID: 8991
		public static BodyTypeDef Thin;

		// Token: 0x04002320 RID: 8992
		public static BodyTypeDef Hulk;

		// Token: 0x04002321 RID: 8993
		public static BodyTypeDef Fat;

		// Token: 0x06003679 RID: 13945 RVA: 0x001D130B File Offset: 0x001CF70B
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDef));
		}
	}
}
