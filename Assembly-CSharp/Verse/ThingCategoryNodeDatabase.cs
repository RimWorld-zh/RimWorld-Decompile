using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E85 RID: 3717
	public static class ThingCategoryNodeDatabase
	{
		// Token: 0x04003A01 RID: 14849
		public static bool initialized = false;

		// Token: 0x04003A02 RID: 14850
		private static TreeNode_ThingCategory rootNode;

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x060057B7 RID: 22455 RVA: 0x002D0A64 File Offset: 0x002CEE64
		public static IEnumerable<TreeNode_ThingCategory> AllThingCategoryNodes
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode.ChildCategoryNodesAndThis;
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x060057B8 RID: 22456 RVA: 0x002D0A84 File Offset: 0x002CEE84
		public static TreeNode_ThingCategory RootNode
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode;
			}
		}

		// Token: 0x060057B9 RID: 22457 RVA: 0x002D0A9E File Offset: 0x002CEE9E
		public static void Clear()
		{
			ThingCategoryNodeDatabase.rootNode = null;
			ThingCategoryNodeDatabase.initialized = false;
		}

		// Token: 0x060057BA RID: 22458 RVA: 0x002D0AB0 File Offset: 0x002CEEB0
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

		// Token: 0x060057BB RID: 22459 RVA: 0x002D0C80 File Offset: 0x002CF080
		private static void SetNestLevelRecursive(TreeNode_ThingCategory node, int nestDepth)
		{
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				thingCategoryDef.treeNode.nestDepth = nestDepth;
				ThingCategoryNodeDatabase.SetNestLevelRecursive(thingCategoryDef.treeNode, nestDepth + 1);
			}
		}
	}
}
