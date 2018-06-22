using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000703 RID: 1795
	public static class BreakdownableUtility
	{
		// Token: 0x06002757 RID: 10071 RVA: 0x00152458 File Offset: 0x00150858
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
