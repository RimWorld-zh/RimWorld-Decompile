using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092C RID: 2348
	[DefOf]
	public static class BodyDefOf
	{
		// Token: 0x06003637 RID: 13879 RVA: 0x001D0835 File Offset: 0x001CEC35
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}

		// Token: 0x04001FFA RID: 8186
		public static BodyDef Human;
	}
}
