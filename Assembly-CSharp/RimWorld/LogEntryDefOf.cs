using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096E RID: 2414
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x06003676 RID: 13942 RVA: 0x001D0F09 File Offset: 0x001CF309
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDef));
		}

		// Token: 0x0400231A RID: 8986
		public static LogEntryDef MeleeAttack;
	}
}
