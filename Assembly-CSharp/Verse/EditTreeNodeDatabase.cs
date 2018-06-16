using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E79 RID: 3705
	public static class EditTreeNodeDatabase
	{
		// Token: 0x0600572E RID: 22318 RVA: 0x002CC10C File Offset: 0x002CA50C
		public static TreeNode_Editor RootOf(object obj)
		{
			for (int i = 0; i < EditTreeNodeDatabase.roots.Count; i++)
			{
				if (EditTreeNodeDatabase.roots[i].obj == obj)
				{
					return EditTreeNodeDatabase.roots[i];
				}
			}
			TreeNode_Editor treeNode_Editor = TreeNode_Editor.NewRootNode(obj);
			EditTreeNodeDatabase.roots.Add(treeNode_Editor);
			return treeNode_Editor;
		}

		// Token: 0x040039C5 RID: 14789
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();
	}
}
