using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000BAD RID: 2989
	public static class ThinkTreeKeyAssigner
	{
		// Token: 0x04002C18 RID: 11288
		private static HashSet<int> assignedKeys = new HashSet<int>();

		// Token: 0x060040E1 RID: 16609 RVA: 0x00224272 File Offset: 0x00222672
		internal static void Reset()
		{
			ThinkTreeKeyAssigner.assignedKeys.Clear();
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x00224280 File Offset: 0x00222680
		public static void AssignKeys(ThinkNode rootNode, int startHash)
		{
			Rand.PushState(startHash);
			foreach (ThinkNode thinkNode in rootNode.ThisAndChildrenRecursive)
			{
				thinkNode.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(thinkNode));
			}
			Rand.PopState();
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x002242F0 File Offset: 0x002226F0
		public static void AssignSingleKey(ThinkNode node, int startHash)
		{
			Rand.PushState(startHash);
			node.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(node));
			Rand.PopState();
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x0022430C File Offset: 0x0022270C
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
	}
}
