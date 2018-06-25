using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E7A RID: 3706
	public static class EditTreeNodeDatabase
	{
		// Token: 0x040039DB RID: 14811
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();

		// Token: 0x06005750 RID: 22352 RVA: 0x002CE034 File Offset: 0x002CC434
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
	}
}
