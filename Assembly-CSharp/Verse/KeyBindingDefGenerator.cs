using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F59 RID: 3929
	public static class KeyBindingDefGenerator
	{
		// Token: 0x06005F32 RID: 24370 RVA: 0x00308440 File Offset: 0x00306840
		public static IEnumerable<KeyBindingCategoryDef> ImpliedKeyBindingCategoryDefs()
		{
			List<KeyBindingCategoryDef> gameUniversalCats = (from d in DefDatabase<KeyBindingCategoryDef>.AllDefs
			where d.isGameUniversal
			select d).ToList<KeyBindingCategoryDef>();
			foreach (DesignationCategoryDef def in DefDatabase<DesignationCategoryDef>.AllDefs)
			{
				KeyBindingCategoryDef catDef = new KeyBindingCategoryDef();
				catDef.defName = "Architect_" + def.defName;
				catDef.label = def.label + " tab";
				catDef.description = "Key bindings for the \"" + def.LabelCap + "\" section of the Architect menu";
				catDef.modContentPack = def.modContentPack;
				catDef.checkForConflicts.AddRange(gameUniversalCats);
				for (int i = 0; i < gameUniversalCats.Count; i++)
				{
					gameUniversalCats[i].checkForConflicts.Add(catDef);
				}
				def.bindingCatDef = catDef;
				yield return catDef;
			}
			yield break;
		}

		// Token: 0x06005F33 RID: 24371 RVA: 0x00308464 File Offset: 0x00306864
		public static IEnumerable<KeyBindingDef> ImpliedKeyBindingDefs()
		{
			foreach (MainButtonDef mainTab in from td in DefDatabase<MainButtonDef>.AllDefs
			orderby td.order
			select td)
			{
				if (mainTab.defaultHotKey != KeyCode.None)
				{
					KeyBindingDef keyDef = new KeyBindingDef();
					keyDef.label = "Toggle " + mainTab.label + " tab";
					keyDef.defName = "MainTab_" + mainTab.defName;
					keyDef.category = KeyBindingCategoryDefOf.MainTabs;
					keyDef.defaultKeyCodeA = mainTab.defaultHotKey;
					keyDef.modContentPack = mainTab.modContentPack;
					mainTab.hotKey = keyDef;
					yield return keyDef;
				}
			}
			yield break;
		}
	}
}
