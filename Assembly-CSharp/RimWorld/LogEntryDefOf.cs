using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000970 RID: 2416
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x0400231B RID: 8987
		public static LogEntryDef MeleeAttack;

		// Token: 0x0600367A RID: 13946 RVA: 0x001D1049 File Offset: 0x001CF449
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDef));
		}
	}
}
