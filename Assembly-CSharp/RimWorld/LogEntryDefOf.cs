using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000972 RID: 2418
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x0600367D RID: 13949 RVA: 0x001D0D21 File Offset: 0x001CF121
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDef));
		}

		// Token: 0x0400231C RID: 8988
		public static LogEntryDef MeleeAttack;
	}
}
