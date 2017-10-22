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
			foreach (DesignationCategoryDef allDef in DefDatabase<DesignationCategoryDef>.AllDefs)
			{
				KeyBindingCategoryDef catDef = new KeyBindingCategoryDef
				{
					defName = "Architect_" + allDef.defName,
					label = allDef.label + " tab",
					description = "Key bindings for the \"" + allDef.LabelCap + "\" section of the Architect menu"
				};
				catDef.checkForConflicts.AddRange(gameUniversalCats);
				for (int i = 0; i < gameUniversalCats.Count; i++)
				{
					gameUniversalCats[i].checkForConflicts.Add(catDef);
				}
				allDef.bindingCatDef = catDef;
				yield return catDef;
			}
		}

		public static IEnumerable<KeyBindingDef> ImpliedKeyBindingDefs()
		{
			foreach (MainButtonDef item in from td in DefDatabase<MainButtonDef>.AllDefs
			orderby td.order
			select td)
			{
				if (item.defaultHotKey != 0)
				{
					KeyBindingDef keyDef = new KeyBindingDef
					{
						label = "Toggle " + item.label + " tab",
						defName = "MainTab_" + item.defName,
						category = KeyBindingCategoryDefOf.MainTabs,
						defaultKeyCodeA = item.defaultHotKey
					};
					item.hotKey = keyDef;
					yield return keyDef;
				}
			}
		}
	}
}
