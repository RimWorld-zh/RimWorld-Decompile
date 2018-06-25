using System;

namespace Verse
{
	// Token: 0x02000D11 RID: 3345
	public static class HediffGrowthModeUtility
	{
		// Token: 0x060049C3 RID: 18883 RVA: 0x00269E50 File Offset: 0x00268250
		public static string GetLabel(this HediffGrowthMode m)
		{
			string result;
			switch (m)
			{
			case HediffGrowthMode.Growing:
				result = "HediffGrowthMode_Growing".Translate();
				break;
			case HediffGrowthMode.Stable:
				result = "HediffGrowthMode_Stable".Translate();
				break;
			case HediffGrowthMode.Remission:
				result = "HediffGrowthMode_Remission".Translate();
				break;
			default:
				throw new ArgumentException();
			}
			return result;
		}
	}
}
