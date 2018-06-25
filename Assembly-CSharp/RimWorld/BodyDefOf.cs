using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092A RID: 2346
	[DefOf]
	public static class BodyDefOf
	{
		// Token: 0x04001FF8 RID: 8184
		public static BodyDef Human;

		// Token: 0x06003634 RID: 13876 RVA: 0x001D0B5D File Offset: 0x001CEF5D
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}
	}
}
