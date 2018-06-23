using System;

namespace RimWorld
{
	// Token: 0x02000933 RID: 2355
	[DefOf]
	public static class ScenarioDefOf
	{
		// Token: 0x0400204D RID: 8269
		public static ScenarioDef Crashlanded;

		// Token: 0x0600363B RID: 13883 RVA: 0x001D0AE3 File Offset: 0x001CEEE3
		static ScenarioDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenarioDefOf));
		}
	}
}
