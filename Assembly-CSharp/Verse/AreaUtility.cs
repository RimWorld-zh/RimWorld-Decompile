using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class AreaUtility
	{
		public static void MakeAllowedAreaListFloatMenu(Action<Area> selAction, AllowedAreaMode mode, bool addNullAreaOption, bool addManageOption, Map map)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (addNullAreaOption)
			{
				list.Add(new FloatMenuOption("NoAreaAllowed".Translate(), (Action)delegate()
				{
					selAction(null);
				}, MenuOptionPriority.High, null, null, 0f, null, null));
			}
			foreach (Area item2 in from a in map.areaManager.AllAreas
			where a.AssignableAsAllowed(mode)
			select a)
			{
				Area localArea = item2;
				FloatMenuOption item = new FloatMenuOption(localArea.Label, (Action)delegate()
				{
					selAction(localArea);
				}, MenuOptionPriority.Default, (Action)delegate
				{
					localArea.MarkForDraw();
				}, null, 0f, null, null);
				list.Add(item);
			}
			if (addManageOption)
			{
				list.Add(new FloatMenuOption("ManageAreas".Translate(), (Action)delegate()
				{
					Find.WindowStack.Add(new Dialog_ManageAreas(map));
				}, MenuOptionPriority.Low, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static string AreaAllowedLabel(Pawn pawn)
		{
			return (pawn.playerSettings == null) ? AreaUtility.AreaAllowedLabel_Area(null) : AreaUtility.AreaAllowedLabel_Area(pawn.playerSettings.EffectiveAreaRestriction);
		}

		public static string AreaAllowedLabel_Area(Area area)
		{
			return (area == null) ? "NoAreaAllowed".Translate() : area.Label;
		}
	}
}
