using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000651 RID: 1617
	public static class ScenSummaryList
	{
		// Token: 0x06002195 RID: 8597 RVA: 0x0011CDF8 File Offset: 0x0011B1F8
		public static string SummaryWithList(Scenario scen, string tag, string intro)
		{
			string text = ScenSummaryList.SummaryList(scen, tag);
			string result;
			if (!text.NullOrEmpty())
			{
				result = "\n" + intro + ":\n" + text;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x0011CE3C File Offset: 0x0011B23C
		private static string SummaryList(Scenario scen, string tag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (ScenPart scenPart in scen.AllParts)
			{
				if (!scenPart.summarized)
				{
					foreach (string str in scenPart.GetSummaryListEntries(tag))
					{
						if (!flag)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append("   -" + str);
						scenPart.summarized = true;
						flag = false;
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
