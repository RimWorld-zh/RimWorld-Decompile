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
				TreeNode_ThingCategory.<>c__Iterator22D <>c__Iterator22D = new TreeNode_ThingCategory.<>c__Iterator22D();
				<>c__Iterator22D.<>f__this = this;
				TreeNode_ThingCategory.<>c__Iterator22D expr_0E = <>c__Iterator22D;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				TreeNode_ThingCategory.<>c__Iterator22E <>c__Iterator22E = new TreeNode_ThingCategory.<>c__Iterator22E();
				<>c__Iterator22E.<>f__this = this;
				TreeNode_ThingCategory.<>c__Iterator22E expr_0E = <>c__Iterator22E;
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
