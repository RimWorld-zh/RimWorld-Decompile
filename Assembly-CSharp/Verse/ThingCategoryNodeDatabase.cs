using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E84 RID: 3716
	public static class ThingCategoryNodeDatabase
	{
		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06005795 RID: 22421 RVA: 0x002CEB3C File Offset: 0x002CCF3C
		public static IEnumerable<TreeNode_ThingCategory> AllThingCategoryNodes
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode.ChildCategoryNodesAndThis;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06005796 RID: 22422 RVA: 0x002CEB5C File Offset: 0x002CCF5C
		public static TreeNode_ThingCategory RootNode
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode;
			}
		}

		// Token: 0x06005797 RID: 22423 RVA: 0x002CEB76 File Offset: 0x002CCF76
		public static void Clear()
		{
			ThingCategoryNodeDatabase.rootNode = null;
			ThingCategoryNodeDatabase.initialized = false;
		}

		// Token: 0x06005798 RID: 22424 RVA: 0x002CEB88 File Offset: 0x002CCF88
		public static void FinalizeInit()
		{
			ThingCategoryNodeDatabase.rootNode = ThingCategoryDefOf.Root.treeNode;
			foreach (ThingCategoryDef thingCategoryDef in DefDatabase<ThingCategoryDef>.AllDefs)
			{
				if (thingCategoryDef.parent != null)
				{
					thingCategoryDef.parent.childCategories.Add(thingCategoryDef);
				}
			}
			ThingCategoryNodeDatabase.SetNestLevelRecursive(ThingCategoryNodeDatabase.rootNode, 0);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.thingCategories != null)
				{
					foreach (ThingCategoryDef thingCategoryDef2 in thingDef.thingCategories)
					{
						thingCategoryDef2.childThingDefs.Add(thingDef);
					}
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in DefDatabase<SpecialThingFilterDef>.AllDefs)
			{
				specialThingFilterDef.parentCategory.childSpecialFilters.Add(specialThingFilterDef);
			}
			if (ThingCategoryNodeDatabase.rootNode.catDef.childCategories.Any<ThingCategoryDef>())
			{
				ThingCategoryNodeDatabase.rootNode.catDef.childCategories[0].treeNode.SetOpen(-1, true);
			}
			ThingCategoryNodeDatabase.initialized = true;
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x002CED58 File Offset: 0x002CD158
		private static void SetNestLevelRecursive(TreeNode_ThingCategory node, int nestDepth)
		{
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				thingCategoryDef.treeNode.nestDepth = nestDepth;
				ThingCategoryNodeDatabase.SetNestLevelRecursive(thingCategoryDef.treeNode, nestDepth + 1);
			}
		}

		// Token: 0x040039EB RID: 14827
		public static bool initialized = false;

		// Token: 0x040039EC RID: 14828
		private static TreeNode_ThingCategory rootNode;
	}
}
