using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000970 RID: 2416
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x04002322 RID: 8994
		public static LogEntryDef MeleeAttack;

		// Token: 0x0600367A RID: 13946 RVA: 0x001D131D File Offset: 0x001CF71D
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDef));
		}
	}
}
