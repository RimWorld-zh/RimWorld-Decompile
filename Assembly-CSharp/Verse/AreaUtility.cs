using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	public static class AreaUtility
	{
		[CompilerGenerated]
		private static Func<Area, bool> <>f__am$cache0;

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

		public static string AreaAllowedLabel(Pawn pawn)
		{
			if (pawn.playerSettings != null)
			{
				return AreaUtility.AreaAllowedLabel_Area(pawn.playerSettings.EffectiveAreaRestriction);
			}
			return AreaUtility.AreaAllowedLabel_Area(null);
		}

		public static string AreaAllowedLabel_Area(Area area)
		{
			if (area != null)
			{
				return area.Label;
			}
			return "NoAreaAllowed".Translate();
		}

		[CompilerGenerated]
		private static bool <MakeAllowedAreaListFloatMenu>m__0(Area a)
		{
			return a.AssignableAsAllowed();
		}

		[CompilerGenerated]
		private sealed class <MakeAllowedAreaListFloatMenu>c__AnonStorey0
		{
			internal Action<Area> selAction;

			internal Map map;

			public <MakeAllowedAreaListFloatMenu>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.selAction(null);
			}

			internal void <>m__1()
			{
				Find.WindowStack.Add(new Dialog_ManageAreas(this.map));
			}
		}

		[CompilerGenerated]
		private sealed class <MakeAllowedAreaListFloatMenu>c__AnonStorey1
		{
			internal Area localArea;

			internal AreaUtility.<MakeAllowedAreaListFloatMenu>c__AnonStorey0 <>f__ref$0;

			public <MakeAllowedAreaListFloatMenu>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$0.selAction(this.localArea);
			}

			internal void <>m__1()
			{
				this.localArea.MarkForDraw();
			}
		}
	}
}
