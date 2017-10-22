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
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			HashSet<int> usedKeys = new HashSet<int>();
			HashSet<ThinkNode> instances = new HashSet<ThinkNode>();
			using (IEnumerator<ThinkNode> enumerator2 = this.thinkRoot.ThisAndChildrenRecursive.GetEnumerator())
			{
				ThinkNode node;
				int key;
				while (true)
				{
					if (enumerator2.MoveNext())
					{
						node = enumerator2.Current;
						key = node.UniqueSaveKey;
						if (key == -1)
						{
							yield return "Thinknode " + node.GetType() + " has invalid save key " + key;
							/*Error: Unable to find new state assignment for yield return*/;
						}
						if (instances.Contains(node))
						{
							yield return "There are two same ThinkNode instances in one think tree (their type is " + node.GetType() + ")";
							/*Error: Unable to find new state assignment for yield return*/;
						}
						if (!usedKeys.Contains(key))
						{
							if (key != -1)
							{
								usedKeys.Add(key);
							}
							instances.Add(node);
							continue;
						}
						break;
					}
					yield break;
				}
				yield return "Two ThinkNodes have the same unique save key " + key + " (one of the nodes is " + node.GetType() + ")";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_02c1:
			/*Error near IL_02c2: Unexpected return in MoveNext()*/;
		}

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
				foreach (ThinkNode item in this.thinkRoot.ChildrenRecursive)
				{
					if (item.UniqueSaveKey == key)
					{
						outNode = item;
						return true;
					}
				}
				result = false;
			}
			return result;
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
