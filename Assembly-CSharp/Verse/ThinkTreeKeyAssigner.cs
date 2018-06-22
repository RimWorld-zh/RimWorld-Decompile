using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000BAB RID: 2987
	public static class ThinkTreeKeyAssigner
	{
		// Token: 0x060040DE RID: 16606 RVA: 0x00224196 File Offset: 0x00222596
		internal static void Reset()
		{
			ThinkTreeKeyAssigner.assignedKeys.Clear();
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x002241A4 File Offset: 0x002225A4
		public static void AssignKeys(ThinkNode rootNode, int startHash)
		{
			Rand.PushState(startHash);
			foreach (ThinkNode thinkNode in rootNode.ThisAndChildrenRecursive)
			{
				thinkNode.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(thinkNode));
			}
			Rand.PopState();
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x00224214 File Offset: 0x00222614
		public static void AssignSingleKey(ThinkNode node, int startHash)
		{
			Rand.PushState(startHash);
			node.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(node));
			Rand.PopState();
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x00224230 File Offset: 0x00222630
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

		// Token: 0x04002C18 RID: 11288
		private static HashSet<int> assignedKeys = new HashSet<int>();
	}
}
