using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E85 RID: 3717
	public class TreeNode_ThingCategory : TreeNode
	{
		// Token: 0x0600579B RID: 22427 RVA: 0x002CEDD8 File Offset: 0x002CD1D8
		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x0600579C RID: 22428 RVA: 0x002CEDE8 File Offset: 0x002CD1E8
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x0600579D RID: 22429 RVA: 0x002CEE08 File Offset: 0x002CD208
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x0600579E RID: 22430 RVA: 0x002CEE28 File Offset: 0x002CD228
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodesAndThis
		{
			get
			{
				foreach (ThingCategoryDef other in this.catDef.ThisAndChildCategoryDefs)
				{
					yield return other.treeNode;
				}
				yield break;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x0600579F RID: 22431 RVA: 0x002CEE54 File Offset: 0x002CD254
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				foreach (ThingCategoryDef other in this.catDef.childCategories)
				{
					yield return other.treeNode;
				}
				yield break;
			}
		}

		// Token: 0x060057A0 RID: 22432 RVA: 0x002CEE80 File Offset: 0x002CD280
		public override string ToString()
		{
			return this.catDef.defName;
		}

		// Token: 0x040039ED RID: 14829
		public ThingCategoryDef catDef;
	}
}
