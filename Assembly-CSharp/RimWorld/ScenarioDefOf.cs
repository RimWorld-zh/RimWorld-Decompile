using System;

namespace RimWorld
{
	// Token: 0x02000935 RID: 2357
	[DefOf]
	public static class ScenarioDefOf
	{
		// Token: 0x0400204D RID: 8269
		public static ScenarioDef Crashlanded;

		// Token: 0x0600363F RID: 13887 RVA: 0x001D0C23 File Offset: 0x001CF023
		static ScenarioDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenarioDefOf));
		}
	}
}
