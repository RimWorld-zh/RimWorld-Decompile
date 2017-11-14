using RimWorld;
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
					KeyBindingCategoryDef catDef = new KeyBindingCategoryDef();
					catDef.defName = "Architect_" + def.defName;
					catDef.label = def.label + " tab";
					catDef.description = "Key bindings for the \"" + def.LabelCap + "\" section of the Architect menu";
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
			IL_01b4:
			/*Error near IL_01b5: Unexpected return in MoveNext()*/;
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
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0171:
			/*Error near IL_0172: Unexpected return in MoveNext()*/;
		}
	}
}
