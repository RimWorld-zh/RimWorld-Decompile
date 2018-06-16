using System;

namespace RimWorld
{
	// Token: 0x02000934 RID: 2356
	[DefOf]
	public static class TrainableDefOf
	{
		// Token: 0x0600363D RID: 13885 RVA: 0x001D07FD File Offset: 0x001CEBFD
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}

		// Token: 0x04002047 RID: 8263
		public static TrainableDef Tameness;

		// Token: 0x04002048 RID: 8264
		public static TrainableDef Obedience;

		// Token: 0x04002049 RID: 8265
		public static TrainableDef Release;
	}
}
