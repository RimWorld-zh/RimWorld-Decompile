using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C01 RID: 3073
	public static class AreaUtility
	{
		// Token: 0x06004327 RID: 17191 RVA: 0x00237148 File Offset: 0x00235548
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

		// Token: 0x06004328 RID: 17192 RVA: 0x002372B0 File Offset: 0x002356B0
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

		// Token: 0x06004329 RID: 17193 RVA: 0x002372EC File Offset: 0x002356EC
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
