using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000653 RID: 1619
	public static class ScenSummaryList
	{
		// Token: 0x06002199 RID: 8601 RVA: 0x0011CBA8 File Offset: 0x0011AFA8
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

		// Token: 0x0600219A RID: 8602 RVA: 0x0011CBEC File Offset: 0x0011AFEC
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
