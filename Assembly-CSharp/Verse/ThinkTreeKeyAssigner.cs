using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	public static class ThinkTreeKeyAssigner
	{
		private static HashSet<int> assignedKeys = new HashSet<int>();

		internal static void Reset()
		{
			ThinkTreeKeyAssigner.assignedKeys.Clear();
		}

		public static void AssignKeys(ThinkNode rootNode, int startHash)
		{
			Rand.PushState(startHash);
			foreach (ThinkNode thinkNode in rootNode.ThisAndChildrenRecursive)
			{
				thinkNode.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(thinkNode));
			}
			Rand.PopState();
		}

		public static void AssignSingleKey(ThinkNode node, int startHash)
		{
			Rand.PushState(startHash);
			node.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(node));
			Rand.PopState();
		}

		private static int NextUnusedKeyFor(ThinkNode node)
		{
			int num = 0;
			while (node != null)
			{
				num = Gen.HashCombineInt(num, GenText.StableStringHash(node.GetType().Name));
				node = node.parent;
			}
			while (ThinkTreeKeyAssigner.assignedKeys.Contains(num))
			{
				num ^= Rand.Int;
			}
			ThinkTreeKeyAssigner.assignedKeys.Add(num);
			return num;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ThinkTreeKeyAssigner()
		{
		}
	}
}
