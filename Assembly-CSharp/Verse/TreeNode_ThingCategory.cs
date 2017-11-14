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
				using (IEnumerator<ThingCategoryDef> enumerator = this.catDef.ThisAndChildCategoryDefs.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ThingCategoryDef other = enumerator.Current;
						yield return other.treeNode;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00c3:
				/*Error near IL_00c4: Unexpected return in MoveNext()*/;
			}
		}

		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				using (List<ThingCategoryDef>.Enumerator enumerator = this.catDef.childCategories.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ThingCategoryDef other = enumerator.Current;
						yield return other.treeNode;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00be:
				/*Error near IL_00bf: Unexpected return in MoveNext()*/;
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
