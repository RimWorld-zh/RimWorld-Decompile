using System;

namespace Verse
{
	public static class HediffGrowthModeUtility
	{
		public static string GetLabel(this HediffGrowthMode m)
		{
			string result;
			switch (m)
			{
			case HediffGrowthMode.Growing:
			{
				result = "HediffGrowthMode_Growing".Translate();
				break;
			}
			case HediffGrowthMode.Stable:
			{
				result = "HediffGrowthMode_Stable".Translate();
				break;
			}
			case HediffGrowthMode.Remission:
			{
				result = "HediffGrowthMode_Remission".Translate();
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}
	}
}
