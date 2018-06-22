using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E83 RID: 3715
	public class TreeNode_ThingCategory : TreeNode
	{
		// Token: 0x060057B9 RID: 22457 RVA: 0x002D09E8 File Offset: 0x002CEDE8
		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x060057BA RID: 22458 RVA: 0x002D09F8 File Offset: 0x002CEDF8
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x060057BB RID: 22459 RVA: 0x002D0A18 File Offset: 0x002CEE18
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x060057BC RID: 22460 RVA: 0x002D0A38 File Offset: 0x002CEE38
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

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x060057BD RID: 22461 RVA: 0x002D0A64 File Offset: 0x002CEE64
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

		// Token: 0x060057BE RID: 22462 RVA: 0x002D0A90 File Offset: 0x002CEE90
		public override string ToString()
		{
			return this.catDef.defName;
		}

		// Token: 0x040039FB RID: 14843
		public ThingCategoryDef catDef;
	}
}
