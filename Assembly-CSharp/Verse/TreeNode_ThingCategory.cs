using System;
using System.Collections.Generic;

namespace Verse
{
	public class TreeNode_ThingCategory : TreeNode
	{
		public ThingCategoryDef catDef;

		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodesAndThis
		{
			get
			{
				TreeNode_ThingCategory.<>c__Iterator235 <>c__Iterator = new TreeNode_ThingCategory.<>c__Iterator235();
				<>c__Iterator.<>f__this = this;
				TreeNode_ThingCategory.<>c__Iterator235 expr_0E = <>c__Iterator;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				TreeNode_ThingCategory.<>c__Iterator236 <>c__Iterator = new TreeNode_ThingCategory.<>c__Iterator236();
				<>c__Iterator.<>f__this = this;
				TreeNode_ThingCategory.<>c__Iterator236 expr_0E = <>c__Iterator;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		public override string ToString()
		{
			return this.catDef.defName;
		}
	}
}
