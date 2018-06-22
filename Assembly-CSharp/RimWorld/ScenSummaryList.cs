using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064F RID: 1615
	public static class ScenSummaryList
	{
		// Token: 0x06002191 RID: 8593 RVA: 0x0011CCA8 File Offset: 0x0011B0A8
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

		// Token: 0x06002192 RID: 8594 RVA: 0x0011CCEC File Offset: 0x0011B0EC
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
