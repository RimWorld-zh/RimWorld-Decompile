using System;

namespace RimWorld
{
	// Token: 0x02000935 RID: 2357
	[DefOf]
	public static class ScenarioDefOf
	{
		// Token: 0x04002054 RID: 8276
		public static ScenarioDef Crashlanded;

		// Token: 0x0600363F RID: 13887 RVA: 0x001D0EF7 File Offset: 0x001CF2F7
		static ScenarioDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenarioDefOf));
		}
	}
}
