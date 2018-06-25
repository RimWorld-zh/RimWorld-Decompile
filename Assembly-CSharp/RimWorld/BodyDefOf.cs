using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092A RID: 2346
	[DefOf]
	public static class BodyDefOf
	{
		// Token: 0x04001FFF RID: 8191
		public static BodyDef Human;

		// Token: 0x06003634 RID: 13876 RVA: 0x001D0E31 File Offset: 0x001CF231
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}
	}
}
