using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	public class ThinkTreeDef : Def
	{
		public ThinkNode thinkRoot;

		[NoTranslate]
		public string insertTag;

		public float insertPriority;

		public override void ResolveReferences()
		{
			this.thinkRoot.ResolveSubnodesAndRecur();
			foreach (ThinkNode item in this.thinkRoot.ThisAndChildrenRecursive)
			{
				item.ResolveReferences();
			}
			ThinkTreeKeyAssigner.AssignKeys(this.thinkRoot, GenText.StableStringHash(base.defName));
			this.ResolveParentNodes(this.thinkRoot);
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			HashSet<int> usedKeys = new HashSet<int>();
			HashSet<ThinkNode> instances = new HashSet<ThinkNode>();
			foreach (ThinkNode item2 in this.thinkRoot.ThisAndChildrenRecursive)
			{
				int key = item2.UniqueSaveKey;
				if (key == -1)
				{
					yield return "Thinknode " + item2.GetType() + " has invalid save key " + key;
				}
				else if (instances.Contains(item2))
				{
					yield return "There are two same ThinkNode instances in one think tree (their type is " + item2.GetType() + ")";
				}
				else if (usedKeys.Contains(key))
				{
					yield return "Two ThinkNodes have the same unique save key " + key + " (one of the nodes is " + item2.GetType() + ")";
				}
				if (key != -1)
				{
					usedKeys.Add(key);
				}
				instances.Add(item2);
			}
		}

		public bool TryGetThinkNodeWithSaveKey(int key, out ThinkNode outNode)
		{
			outNode = null;
			if (key == -1)
			{
				return false;
			}
			if (key == this.thinkRoot.UniqueSaveKey)
			{
				outNode = this.thinkRoot;
				return true;
			}
			foreach (ThinkNode item in this.thinkRoot.ChildrenRecursive)
			{
				if (item.UniqueSaveKey == key)
				{
					outNode = item;
					return true;
				}
			}
			return false;
		}

		private void ResolveParentNodes(ThinkNode node)
		{
			for (int i = 0; i < node.subNodes.Count; i++)
			{
				if (node.subNodes[i].parent != null)
				{
					Log.Warning("Think node " + node.subNodes[i] + " from think tree " + base.defName + " already has a parent node (" + node.subNodes[i].parent + "). This means that it's referenced by more than one think tree (should have been copied instead).");
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
