using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E85 RID: 3717
	public class TreeNode_ThingCategory : TreeNode
	{
		// Token: 0x040039FB RID: 14843
		public ThingCategoryDef catDef;

		// Token: 0x060057BD RID: 22461 RVA: 0x002D0B14 File Offset: 0x002CEF14
		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x060057BE RID: 22462 RVA: 0x002D0B24 File Offset: 0x002CEF24
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x060057BF RID: 22463 RVA: 0x002D0B44 File Offset: 0x002CEF44
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x060057C0 RID: 22464 RVA: 0x002D0B64 File Offset: 0x002CEF64
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

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x060057C1 RID: 22465 RVA: 0x002D0B90 File Offset: 0x002CEF90
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

		// Token: 0x060057C2 RID: 22466 RVA: 0x002D0BBC File Offset: 0x002CEFBC
		public override string ToString()
		{
			return this.catDef.defName;
		}
	}
}
