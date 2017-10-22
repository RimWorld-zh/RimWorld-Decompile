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
				foreach (ThingCategoryDef thisAndChildCategoryDef in this.catDef.ThisAndChildCategoryDefs)
				{
					yield return thisAndChildCategoryDef.treeNode;
				}
			}
		}

		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				List<ThingCategoryDef>.Enumerator enumerator = this.catDef.childCategories.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ThingCategoryDef other = enumerator.Current;
						yield return other.treeNode;
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
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
