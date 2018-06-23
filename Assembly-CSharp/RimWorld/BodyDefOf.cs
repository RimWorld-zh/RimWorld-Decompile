using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000928 RID: 2344
	[DefOf]
	public static class BodyDefOf
	{
		// Token: 0x04001FF8 RID: 8184
		public static BodyDef Human;

		// Token: 0x06003630 RID: 13872 RVA: 0x001D0A1D File Offset: 0x001CEE1D
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}
	}
}
