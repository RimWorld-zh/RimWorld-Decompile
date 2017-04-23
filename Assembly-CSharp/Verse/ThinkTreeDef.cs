using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			foreach (ThinkNode current in this.thinkRoot.ThisAndChildrenRecursive)
			{
				current.ResolveReferences();
			}
			ThinkTreeKeyAssigner.AssignKeys(this.thinkRoot, GenText.StableStringHash(this.defName));
			this.ResolveParentNodes(this.thinkRoot);
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			ThinkTreeDef.<ConfigErrors>c__Iterator1E0 <ConfigErrors>c__Iterator1E = new ThinkTreeDef.<ConfigErrors>c__Iterator1E0();
			<ConfigErrors>c__Iterator1E.<>f__this = this;
			ThinkTreeDef.<ConfigErrors>c__Iterator1E0 expr_0E = <ConfigErrors>c__Iterator1E;
			expr_0E.$PC = -2;
			return expr_0E;
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
			foreach (ThinkNode current in this.thinkRoot.ChildrenRecursive)
			{
				if (current.UniqueSaveKey == key)
				{
					outNode = current;
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
					Log.Warning(string.Concat(new object[]
					{
						"Think node ",
						node.subNodes[i],
						" from think tree ",
						this.defName,
						" already has a parent node (",
						node.subNodes[i].parent,
						"). This means that it's referenced by more than one think tree (should have been copied instead)."
					}));
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
