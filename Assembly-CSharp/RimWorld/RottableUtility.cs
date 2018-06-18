using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000734 RID: 1844
	public static class RottableUtility
	{
		// Token: 0x0600289A RID: 10394 RVA: 0x0015AAC4 File Offset: 0x00158EC4
		public static bool IsNotFresh(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage != RotStage.Fresh;
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x0015AAF8 File Offset: 0x00158EF8
		public static bool IsDessicated(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage == RotStage.Dessicated;
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x0015AB28 File Offset: 0x00158F28
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
