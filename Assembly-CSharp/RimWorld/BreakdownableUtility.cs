using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000705 RID: 1797
	public static class BreakdownableUtility
	{
		// Token: 0x0600275A RID: 10074 RVA: 0x00152808 File Offset: 0x00150C08
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
