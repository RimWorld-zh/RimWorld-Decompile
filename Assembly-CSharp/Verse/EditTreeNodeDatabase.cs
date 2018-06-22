using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E77 RID: 3703
	public static class EditTreeNodeDatabase
	{
		// Token: 0x0600574C RID: 22348 RVA: 0x002CDD1C File Offset: 0x002CC11C
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

		// Token: 0x040039D3 RID: 14803
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();
	}
}
