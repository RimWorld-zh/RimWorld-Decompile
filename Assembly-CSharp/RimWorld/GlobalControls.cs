using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GlobalControls
	{
		public const float Width = 200f;

		private WidgetRow rowVisibility = new WidgetRow();

		public void GlobalControlsOnGUI()
		{
			if (Event.current.type != EventType.Layout)
			{
				float num = (float)((float)UI.screenWidth - 200.0);
				float num2 = (float)UI.screenHeight;
				num2 = (float)(num2 - 35.0);
				GenUI.DrawTextWinterShadow(new Rect((float)(UI.screenWidth - 270), (float)(UI.screenHeight - 450), 270f, 450f));
				num2 = (float)(num2 - 4.0);
				GlobalControlsUtility.DoPlaySettings(this.rowVisibility, false, ref num2);
				num2 = (float)(num2 - 4.0);
				GlobalControlsUtility.DoTimespeedControls(num, 200f, ref num2);
				num2 = (float)(num2 - 4.0);
				GlobalControlsUtility.DoDate(num, 200f, ref num2);
				Rect rect = new Rect((float)(num - 30.0), (float)(num2 - 26.0), 230f, 26f);
				Find.VisibleMap.weatherManager.DoWeatherGUI(rect);
				num2 -= rect.height;
				Rect rect2 = new Rect((float)(num - 100.0), (float)(num2 - 26.0), 293f, 26f);
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect2, GlobalControls.TemperatureString());
				Text.Anchor = TextAnchor.UpperLeft;
				num2 = (float)(num2 - 26.0);
				float num3 = 230f;
				float num4 = Find.VisibleMap.gameConditionManager.TotalHeightAt((float)(num3 - 15.0));
				Rect rect3 = new Rect((float)(num - 30.0), num2 - num4, num3, num4);
				Find.VisibleMap.gameConditionManager.DoConditionsUI(rect3);
				num2 -= rect3.height;
				if (Prefs.ShowRealtimeClock)
				{
					GlobalControlsUtility.DoRealtimeClock(num, 200f, ref num2);
				}
				if (Find.VisibleMap.info.parent.ForceExitAndRemoveMapCountdownActive)
				{
					Rect rect4 = new Rect(num, (float)(num2 - 26.0), 193f, 26f);
					Text.Anchor = TextAnchor.MiddleRight;
					GlobalControls.DoCountdownTimer(rect4);
					Text.Anchor = TextAnchor.UpperLeft;
					num2 = (float)(num2 - 26.0);
				}
				num2 = (float)(num2 - 10.0);
				Find.LetterStack.LettersOnGUI(num2);
			}
		}

		private static string TemperatureString()
		{
			IntVec3 c;
			IntVec3 intVec = c = UI.MouseCell();
			Room room = intVec.GetRoom(Find.VisibleMap, RegionType.Set_All);
			if (room == null)
			{
				for (int i = 0; i < 9; i++)
				{
					IntVec3 intVec2 = intVec + GenAdj.AdjacentCellsAndInside[i];
					Room room2;
					if (intVec2.InBounds(Find.VisibleMap))
					{
						room2 = intVec2.GetRoom(Find.VisibleMap, RegionType.Set_All);
						if (room2 != null)
						{
							if (!room2.PsychologicallyOutdoors && !room2.UsesOutdoorTemperature)
							{
								goto IL_00a8;
							}
							if (!room2.PsychologicallyOutdoors && (room == null || room.PsychologicallyOutdoors))
							{
								goto IL_00a8;
							}
							if (room2.PsychologicallyOutdoors && room == null)
								goto IL_00a8;
						}
					}
					continue;
					IL_00a8:
					c = intVec2;
					room = room2;
				}
			}
			if (room == null && intVec.InBounds(Find.VisibleMap))
			{
				Building edifice = intVec.GetEdifice(Find.VisibleMap);
				if (edifice != null)
				{
					CellRect.CellRectIterator iterator = edifice.OccupiedRect().ExpandedBy(1).ClipInsideMap(Find.VisibleMap).GetIterator();
					while (!iterator.Done())
					{
						IntVec3 current = iterator.Current;
						room = current.GetRoom(Find.VisibleMap, RegionType.Set_All);
						if (room != null && !room.PsychologicallyOutdoors)
						{
							c = current;
							break;
						}
						iterator.MoveNext();
					}
				}
			}
			string str = (!c.InBounds(Find.VisibleMap) || c.Fogged(Find.VisibleMap) || room == null || room.PsychologicallyOutdoors) ? "Outdoors".Translate() : ((room.OpenRoofCount != 0) ? ("IndoorsUnroofed".Translate() + " (" + room.OpenRoofCount.ToStringCached() + ")") : "Indoors".Translate());
			float celsiusTemp = (room != null && !c.Fogged(Find.VisibleMap)) ? room.Temperature : Find.VisibleMap.mapTemperature.OutdoorTemp;
			return str + " " + celsiusTemp.ToStringTemperature("F0");
		}

		private static void DoCountdownTimer(Rect rect)
		{
			string forceExitAndRemoveMapCountdownTimeLeftString = Find.VisibleMap.info.parent.ForceExitAndRemoveMapCountdownTimeLeftString;
			string text = "ForceExitAndRemoveMapCountdown".Translate(forceExitAndRemoveMapCountdownTimeLeftString);
			Vector2 vector = Text.CalcSize(text);
			float x = vector.x;
			Rect rect2 = new Rect(rect.xMax - x, rect.y, x, rect.height);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			TooltipHandler.TipRegion(rect2, "ForceExitAndRemoveMapCountdownTip".Translate(forceExitAndRemoveMapCountdownTimeLeftString));
			Widgets.Label(rect2, text);
		}
	}
}
