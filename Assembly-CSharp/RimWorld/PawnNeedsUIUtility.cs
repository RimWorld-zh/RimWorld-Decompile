using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081A RID: 2074
	public static class PawnNeedsUIUtility
	{
		// Token: 0x06002E5A RID: 11866 RVA: 0x0018AD97 File Offset: 0x00189197
		public static void SortInDisplayOrder(List<Need> needs)
		{
			needs.Sort((Need a, Need b) => b.def.listPriority.CompareTo(a.def.listPriority));
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x0018ADC0 File Offset: 0x001891C0
		public static Thought GetLeadingThoughtInGroup(List<Thought> thoughtsInGroup)
		{
			Thought result = null;
			int num = -1;
			for (int i = 0; i < thoughtsInGroup.Count; i++)
			{
				if (thoughtsInGroup[i].CurStageIndex > num)
				{
					num = thoughtsInGroup[i].CurStageIndex;
					result = thoughtsInGroup[i];
				}
			}
			return result;
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x0018AE1C File Offset: 0x0018921C
		public static void GetThoughtGroupsInDisplayOrder(Need_Mood mood, List<Thought> outThoughtGroupsPresent)
		{
			mood.thoughts.GetDistinctMoodThoughtGroups(outThoughtGroupsPresent);
			for (int i = outThoughtGroupsPresent.Count - 1; i >= 0; i--)
			{
				if (!outThoughtGroupsPresent[i].VisibleInNeedsTab)
				{
					outThoughtGroupsPresent.RemoveAt(i);
				}
			}
			outThoughtGroupsPresent.SortByDescending((Thought t) => mood.thoughts.MoodOffsetOfGroup(t), (Thought t) => t.GetHashCode());
		}
	}
}
