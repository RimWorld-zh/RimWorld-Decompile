using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class KeyBindingDefGenerator
	{
		public static IEnumerable<KeyBindingCategoryDef> ImpliedKeyBindingCategoryDefs()
		{
			List<KeyBindingCategoryDef> gameUniversalCats = (from d in DefDatabase<KeyBindingCategoryDef>.AllDefs
			where d.isGameUniversal
			select d).ToList();
			using (IEnumerator<DesignationCategoryDef> enumerator = DefDatabase<DesignationCategoryDef>.AllDefs.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					DesignationCategoryDef def = enumerator.Current;
					KeyBindingCategoryDef catDef = new KeyBindingCategoryDef
					{
						defName = "Architect_" + def.defName,
						label = def.label + " tab",
						description = "Key bindings for the \"" + def.LabelCap + "\" section of the Architect menu"
					};
					catDef.checkForConflicts.AddRange(gameUniversalCats);
					for (int i = 0; i < gameUniversalCats.Count; i++)
					{
						gameUniversalCats[i].checkForConflicts.Add(catDef);
					}
					def.bindingCatDef = catDef;
					yield return catDef;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_01ba:
			/*Error near IL_01bb: Unexpected return in MoveNext()*/;
		}

		public static IEnumerable<KeyBindingDef> ImpliedKeyBindingDefs()
		{
			using (IEnumerator<MainButtonDef> enumerator = (from td in DefDatabase<MainButtonDef>.AllDefs
			orderby td.order
			select td).GetEnumerator())
			{
				MainButtonDef mainTab;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						mainTab = enumerator.Current;
						if (mainTab.defaultHotKey != 0)
							break;
						continue;
					}
					yield break;
				}
				yield return mainTab.hotKey = new KeyBindingDef
				{
					label = "Toggle " + mainTab.label + " tab",
					defName = "MainTab_" + mainTab.defName,
					category = KeyBindingCategoryDefOf.MainTabs,
					defaultKeyCodeA = mainTab.defaultHotKey
				};
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_0175:
			/*Error near IL_0176: Unexpected return in MoveNext()*/;
		}
	}
}
