using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000BAE RID: 2990
	public class ThinkTreeDef : Def
	{
		// Token: 0x04002C19 RID: 11289
		public ThinkNode thinkRoot;

		// Token: 0x04002C1A RID: 11290
		[NoTranslate]
		public string insertTag;

		// Token: 0x04002C1B RID: 11291
		public float insertPriority;

		// Token: 0x060040E7 RID: 16615 RVA: 0x00224390 File Offset: 0x00222790
		public override void ResolveReferences()
		{
			this.thinkRoot.ResolveSubnodesAndRecur();
			foreach (ThinkNode thinkNode in this.thinkRoot.ThisAndChildrenRecursive)
			{
				thinkNode.ResolveReferences();
			}
			ThinkTreeKeyAssigner.AssignKeys(this.thinkRoot, GenText.StableStringHash(this.defName));
			this.ResolveParentNodes(this.thinkRoot);
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x00224420 File Offset: 0x00222820
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			HashSet<int> usedKeys = new HashSet<int>();
			HashSet<ThinkNode> instances = new HashSet<ThinkNode>();
			foreach (ThinkNode node in this.thinkRoot.ThisAndChildrenRecursive)
			{
				int key = node.UniqueSaveKey;
				if (key == -1)
				{
					yield return string.Concat(new object[]
					{
						"Thinknode ",
						node.GetType(),
						" has invalid save key ",
						key
					});
				}
				else if (instances.Contains(node))
				{
					yield return "There are two same ThinkNode instances in one think tree (their type is " + node.GetType() + ")";
				}
				else if (usedKeys.Contains(key))
				{
					yield return string.Concat(new object[]
					{
						"Two ThinkNodes have the same unique save key ",
						key,
						" (one of the nodes is ",
						node.GetType(),
						")"
					});
				}
				if (key != -1)
				{
					usedKeys.Add(key);
				}
				instances.Add(node);
			}
			yield break;
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x0022444C File Offset: 0x0022284C
		public bool TryGetThinkNodeWithSaveKey(int key, out ThinkNode outNode)
		{
			outNode = null;
			bool result;
			if (key == -1)
			{
				result = false;
			}
			else if (key == this.thinkRoot.UniqueSaveKey)
			{
				outNode = this.thinkRoot;
				result = true;
			}
			else
			{
				foreach (ThinkNode thinkNode in this.thinkRoot.ChildrenRecursive)
				{
					if (thinkNode.UniqueSaveKey == key)
					{
						outNode = thinkNode;
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x002244F8 File Offset: 0x002228F8
		private void ResolveParentNodes(ThinkNode node)
		{
			for (int i = 0; i < node.subNodes.Count; i++)
			{
				if (node.subNodes[i].parent != null)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Think node ",
						node.subNodes[i],
						" from think tree ",
						this.defName,
						" already has a parent node (",
						node.subNodes[i].parent,
						"). This means that it's referenced by more than one think tree (should have been copied instead)."
					}), false);
				}
				else
				{
					node.subNodes[i].parent = node;
					this.ResolveParentNodes(node.subNodes[i]);
				}
			}
		}
	}
}
