using System;

namespace RimWorld
{
	// Token: 0x02000932 RID: 2354
	[DefOf]
	public static class TrainableDefOf
	{
		// Token: 0x0400204C RID: 8268
		public static TrainableDef Tameness;

		// Token: 0x0400204D RID: 8269
		public static TrainableDef Obedience;

		// Token: 0x0400204E RID: 8270
		public static TrainableDef Release;

		// Token: 0x0600363C RID: 13884 RVA: 0x001D0EC1 File Offset: 0x001CF2C1
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}
	}
}
