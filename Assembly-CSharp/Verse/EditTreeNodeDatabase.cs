using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E79 RID: 3705
	public static class EditTreeNodeDatabase
	{
		// Token: 0x040039D3 RID: 14803
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();

		// Token: 0x06005750 RID: 22352 RVA: 0x002CDE48 File Offset: 0x002CC248
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
