using System;

namespace Verse
{
	// Token: 0x02000D0F RID: 3343
	public static class HediffGrowthModeUtility
	{
		// Token: 0x060049C0 RID: 18880 RVA: 0x00269D74 File Offset: 0x00268174
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
