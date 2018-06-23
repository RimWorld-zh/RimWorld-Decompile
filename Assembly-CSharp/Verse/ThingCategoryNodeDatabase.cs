using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E82 RID: 3714
	public static class ThingCategoryNodeDatabase
	{
		// Token: 0x040039F9 RID: 14841
		public static bool initialized = false;

		// Token: 0x040039FA RID: 14842
		private static TreeNode_ThingCategory rootNode;

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x060057B3 RID: 22451 RVA: 0x002D074C File Offset: 0x002CEB4C
		public static IEnumerable<TreeNode_ThingCategory> AllThingCategoryNodes
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode.ChildCategoryNodesAndThis;
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x060057B4 RID: 22452 RVA: 0x002D076C File Offset: 0x002CEB6C
		public static TreeNode_ThingCategory RootNode
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode;
			}
		}

		// Token: 0x060057B5 RID: 22453 RVA: 0x002D0786 File Offset: 0x002CEB86
		public static void Clear()
		{
			ThingCategoryNodeDatabase.rootNode = null;
			ThingCategoryNodeDatabase.initialized = false;
		}

		// Token: 0x060057B6 RID: 22454 RVA: 0x002D0798 File Offset: 0x002CEB98
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

		// Token: 0x060057B7 RID: 22455 RVA: 0x002D0968 File Offset: 0x002CED68
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
