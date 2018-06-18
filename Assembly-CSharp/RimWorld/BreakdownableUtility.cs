using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000707 RID: 1799
	public static class BreakdownableUtility
	{
		// Token: 0x0600275F RID: 10079 RVA: 0x001522B4 File Offset: 0x001506B4
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
