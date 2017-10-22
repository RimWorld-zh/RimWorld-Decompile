using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse
{
	public static class ThingCategoryNodeDatabase
	{
		public static bool initialized;

		private static TreeNode_ThingCategory rootNode;

		public static IEnumerable<TreeNode_ThingCategory> AllThingCategoryNodes
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode.ChildCategoryNodesAndThis;
			}
		}

		public static TreeNode_ThingCategory RootNode
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode;
			}
		}

		public static void Clear()
		{
			ThingCategoryNodeDatabase.rootNode = null;
			ThingCategoryNodeDatabase.initialized = false;
		}

		public static void FinalizeInit()
		{
			ThingCategoryNodeDatabase.rootNode = ThingCategoryDefOf.Root.treeNode;
			foreach (ThingCategoryDef allDef in DefDatabase<ThingCategoryDef>.AllDefs)
			{
				if (allDef.parent != null)
				{
					allDef.parent.childCategories.Add(allDef);
				}
			}
			ThingCategoryNodeDatabase.SetNestLevelRecursive(ThingCategoryNodeDatabase.rootNode, 0);
			foreach (ThingDef allDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef2.thingCategories != null)
				{
					List<ThingCategoryDef>.Enumerator enumerator3 = allDef2.thingCategories.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							ThingCategoryDef current3 = enumerator3.Current;
							current3.childThingDefs.Add(allDef2);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator3).Dispose();
					}
				}
			}
			foreach (SpecialThingFilterDef allDef3 in DefDatabase<SpecialThingFilterDef>.AllDefs)
			{
				allDef3.parentCategory.childSpecialFilters.Add(allDef3);
			}
			ThingCategoryNodeDatabase.rootNode.catDef.childCategories[0].treeNode.SetOpen(-1, true);
			ThingCategoryNodeDatabase.initialized = true;
		}

		private static void SetNestLevelRecursive(TreeNode_ThingCategory node, int nestDepth)
		{
			List<ThingCategoryDef>.Enumerator enumerator = node.catDef.childCategories.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ThingCategoryDef current = enumerator.Current;
					current.treeNode.nestDepth = nestDepth;
					ThingCategoryNodeDatabase.SetNestLevelRecursive(current.treeNode, nestDepth + 1);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
