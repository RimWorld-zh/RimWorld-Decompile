using System;

namespace Verse
{
	// Token: 0x02000D13 RID: 3347
	public static class HediffGrowthModeUtility
	{
		// Token: 0x060049B1 RID: 18865 RVA: 0x00268968 File Offset: 0x00266D68
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
