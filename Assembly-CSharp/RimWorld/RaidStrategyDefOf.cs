using System;

namespace RimWorld
{
	// Token: 0x02000936 RID: 2358
	[DefOf]
	public static class RaidStrategyDefOf
	{
		// Token: 0x0400204E RID: 8270
		public static RaidStrategyDef ImmediateAttack;

		// Token: 0x06003640 RID: 13888 RVA: 0x001D0C35 File Offset: 0x001CF035
		static RaidStrategyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RaidStrategyDefOf));
		}
	}
}
