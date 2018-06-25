using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000732 RID: 1842
	public static class RottableUtility
	{
		// Token: 0x06002896 RID: 10390 RVA: 0x0015B04C File Offset: 0x0015944C
		public static bool IsNotFresh(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage != RotStage.Fresh;
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x0015B080 File Offset: 0x00159480
		public static bool IsDessicated(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage == RotStage.Dessicated;
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x0015B0B0 File Offset: 0x001594B0
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
