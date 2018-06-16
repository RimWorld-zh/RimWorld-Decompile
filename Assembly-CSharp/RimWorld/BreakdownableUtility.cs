using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000707 RID: 1799
	public static class BreakdownableUtility
	{
		// Token: 0x0600275D RID: 10077 RVA: 0x0015223C File Offset: 0x0015063C
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
