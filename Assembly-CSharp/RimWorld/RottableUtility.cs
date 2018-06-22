using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000730 RID: 1840
	public static class RottableUtility
	{
		// Token: 0x06002893 RID: 10387 RVA: 0x0015AC9C File Offset: 0x0015909C
		public static bool IsNotFresh(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage != RotStage.Fresh;
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x0015ACD0 File Offset: 0x001590D0
		public static bool IsDessicated(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage == RotStage.Dessicated;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x0015AD00 File Offset: 0x00159100
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
