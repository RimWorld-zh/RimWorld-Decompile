using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000705 RID: 1797
	public static class BreakdownableUtility
	{
		// Token: 0x0600275B RID: 10075 RVA: 0x001525A8 File Offset: 0x001509A8
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
