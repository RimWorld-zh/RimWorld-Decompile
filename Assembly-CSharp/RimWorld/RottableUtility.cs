using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000734 RID: 1844
	public static class RottableUtility
	{
		// Token: 0x06002898 RID: 10392 RVA: 0x0015AA30 File Offset: 0x00158E30
		public static bool IsNotFresh(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage != RotStage.Fresh;
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x0015AA64 File Offset: 0x00158E64
		public static bool IsDessicated(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage == RotStage.Dessicated;
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x0015AA94 File Offset: 0x00158E94
		public static RotStage GetRotStage(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			RotStage result;
			if (compRottable == null)
			{
				result = RotStage.Fresh;
			}
			else
			{
				result = compRottable.Stage;
			}
			return result;
		}
	}
}
