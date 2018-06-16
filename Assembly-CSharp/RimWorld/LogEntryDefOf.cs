using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000972 RID: 2418
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x0600367B RID: 13947 RVA: 0x001D0C59 File Offset: 0x001CF059
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDef));
		}

		// Token: 0x0400231C RID: 8988
		public static LogEntryDef MeleeAttack;
	}
}
