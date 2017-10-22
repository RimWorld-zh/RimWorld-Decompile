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
			Rand.PushState();
			Rand.Seed = startHash;
			foreach (ThinkNode item in rootNode.ThisAndChildrenRecursive)
			{
				item.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKey());
			}
			Rand.PopState();
		}

		public static void AssignSingleKey(ThinkNode node, int startHash)
		{
			Rand.PushState();
			Rand.Seed = startHash;
			node.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKey());
			Rand.PopState();
		}

		private static int NextUnusedKey()
		{
			int num;
			while (true)
			{
				num = Rand.Range(1, 2147483647);
				if (!ThinkTreeKeyAssigner.assignedKeys.Contains(num))
					break;
			}
			ThinkTreeKeyAssigner.assignedKeys.Add(num);
			return num;
		}
	}
}
