using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081C RID: 2076
	public static class PawnNeedsUIUtility
	{
		// Token: 0x06002E5B RID: 11867 RVA: 0x0018A9D3 File Offset: 0x00188DD3
		public static void SortInDisplayOrder(List<Need> needs)
		{
			needs.Sort((Need a, Need b) => b.def.listPriority.CompareTo(a.def.listPriority));
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x0018A9FC File Offset: 0x00188DFC
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

		// Token: 0x06002E5D RID: 11869 RVA: 0x0018AA58 File Offset: 0x00188E58
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
