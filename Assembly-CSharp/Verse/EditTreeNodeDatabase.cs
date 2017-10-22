using System.Collections.Generic;

namespace Verse
{
	public static class EditTreeNodeDatabase
	{
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();

		public static TreeNode_Editor RootOf(object obj)
		{
			int num = 0;
			TreeNode_Editor result;
			while (true)
			{
				if (num < EditTreeNodeDatabase.roots.Count)
				{
					if (EditTreeNodeDatabase.roots[num].obj == obj)
					{
						result = EditTreeNodeDatabase.roots[num];
						break;
					}
					num++;
					continue;
				}
				TreeNode_Editor treeNode_Editor = TreeNode_Editor.NewRootNode(obj);
				EditTreeNodeDatabase.roots.Add(treeNode_Editor);
				result = treeNode_Editor;
				break;
			}
			return result;
		}
	}
}
