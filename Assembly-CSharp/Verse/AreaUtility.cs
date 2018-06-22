using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BFD RID: 3069
	public static class AreaUtility
	{
		// Token: 0x0600432E RID: 17198 RVA: 0x002384B0 File Offset: 0x002368B0
		public static void MakeAllowedAreaListFloatMenu(Action<Area> selAction, bool addNullAreaOption, bool addManageOption, Map map)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (addNullAreaOption)
			{
				list.Add(new FloatMenuOption("NoAreaAllowed".Translate(), delegate()
				{
					selAction(null);
				}, MenuOptionPriority.High, null, null, 0f, null, null));
			}
			foreach (Area localArea2 in from a in map.areaManager.AllAreas
			where a.AssignableAsAllowed()
			select a)
			{
				Area localArea = localArea2;
				FloatMenuOption item = new FloatMenuOption(localArea.Label, delegate()
				{
					selAction(localArea);
				}, MenuOptionPriority.Default, delegate()
				{
					localArea.MarkForDraw();
				}, null, 0f, null, null);
				list.Add(item);
			}
			if (addManageOption)
			{
				list.Add(new FloatMenuOption("ManageAreas".Translate(), delegate()
				{
					Find.WindowStack.Add(new Dialog_ManageAreas(map));
				}, MenuOptionPriority.Low, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x00238618 File Offset: 0x00236A18
		public static string AreaAllowedLabel(Pawn pawn)
		{
			string result;
			if (pawn.playerSettings != null)
			{
				result = AreaUtility.AreaAllowedLabel_Area(pawn.playerSettings.EffectiveAreaRestriction);
			}
			else
			{
				result = AreaUtility.AreaAllowedLabel_Area(null);
			}
			return result;
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x00238654 File Offset: 0x00236A54
		public static string AreaAllowedLabel_Area(Area area)
		{
			string result;
			if (area != null)
			{
				result = area.Label;
			}
			else
			{
				result = "NoAreaAllowed".Translate();
			}
			return result;
		}
	}
}
