using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class PawnNeedsUIUtility
	{
		[CompilerGenerated]
		private static Comparison<Need> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Thought, int> <>f__am$cache1;

		public static void SortInDisplayOrder(List<Need> needs)
		{
			needs.Sort((Need a, Need b) => b.def.listPriority.CompareTo(a.def.listPriority));
		}

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

		[CompilerGenerated]
		private static int <SortInDisplayOrder>m__0(Need a, Need b)
		{
			return b.def.listPriority.CompareTo(a.def.listPriority);
		}

		[CompilerGenerated]
		private static int <GetThoughtGroupsInDisplayOrder>m__1(Thought t)
		{
			return t.GetHashCode();
		}

		[CompilerGenerated]
		private sealed class <GetThoughtGroupsInDisplayOrder>c__AnonStorey0
		{
			internal Need_Mood mood;

			public <GetThoughtGroupsInDisplayOrder>c__AnonStorey0()
			{
			}

			internal float <>m__0(Thought t)
			{
				return this.mood.thoughts.MoodOffsetOfGroup(t);
			}
		}
	}
}
