using System;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x02000840 RID: 2112
	public class GlobalControls
	{
		// Token: 0x040019D7 RID: 6615
		public const float Width = 200f;

		// Token: 0x040019D8 RID: 6616
		private WidgetRow rowVisibility = new WidgetRow();

		// Token: 0x06002FCB RID: 12235 RVA: 0x0019E980 File Offset: 0x0019CD80
		public void GlobalControlsOnGUI()
		{
			if (Event.current.type != EventType.Layout)
			{
				float num = (float)UI.screenWidth - 200f;
				float num2 = (float)UI.screenHeight;
				num2 -= 35f;
				GenUI.DrawTextWinterShadow(new Rect((float)(UI.screenWidth - 270), (float)(UI.screenHeight - 450), 270f, 450f));
				num2 -= 4f;
				Profiler.BeginSample("Play settings");
				GlobalControlsUtility.DoPlaySettings(this.rowVisibility, false, ref num2);
				Profiler.EndSample();
				num2 -= 4f;
				Profiler.BeginSample("Timespeed controls");
				GlobalControlsUtility.DoTimespeedControls(num, 200f, ref num2);
				Profiler.EndSample();
				num2 -= 4f;
				Profiler.BeginSample("Date");
				GlobalControlsUtility.DoDate(num, 200f, ref num2);
				Profiler.EndSample();
				Profiler.BeginSample("Weather");
				Rect rect = new Rect(num - 30f, num2 - 26f, 230f, 26f);
				Find.CurrentMap.weatherManager.DoWeatherGUI(rect);
				num2 -= rect.height;
				Profiler.EndSample();
				Profiler.BeginSample("Temperature");
				Rect rect2 = new Rect(num - 100f, num2 - 26f, 293f, 26f);
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect2, GlobalControls.TemperatureString());
				Text.Anchor = TextAnchor.UpperLeft;
				num2 -= 26f;
				Profiler.EndSample();
				Profiler.BeginSample("Conditions");
				float num3 = 230f;
				float num4 = Find.CurrentMap.gameConditionManager.TotalHeightAt(num3 - 15f);
				Rect rect3 = new Rect(num - 30f, num2 - num4, num3, num4);
				Find.CurrentMap.gameConditionManager.DoConditionsUI(rect3);
				num2 -= rect3.height;
				Profiler.EndSample();
				if (Prefs.ShowRealtimeClock)
				{
					Profiler.BeginSample("RealtimeClock");
					GlobalControlsUtility.DoRealtimeClock(num, 200f, ref num2);
					Profiler.EndSample();
				}
				TimedForcedExit component = Find.CurrentMap.Parent.GetComponent<TimedForcedExit>();
				if (component != null && component.ForceExitAndRemoveMapCountdownActive)
				{
					Profiler.BeginSample("ForceExitAndRemoveMapCountdown");
					Rect rect4 = new Rect(num, num2 - 26f, 193f, 26f);
					Text.Anchor = TextAnchor.MiddleRight;
					GlobalControls.DoCountdownTimer(rect4, component);
					Text.Anchor = TextAnchor.UpperLeft;
					num2 -= 26f;
					Profiler.EndSample();
				}
				Profiler.BeginSample("Letters");
				num2 -= 10f;
				Find.LetterStack.LettersOnGUI(num2);
				Profiler.EndSample();
			}
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x0019EC10 File Offset: 0x0019D010
		private static string TemperatureString()
		{
			IntVec3 intVec = UI.MouseCell();
			IntVec3 c = intVec;
			Room room = intVec.GetRoom(Find.CurrentMap, RegionType.Set_All);
			if (room == null)
			{
				for (int i = 0; i < 9; i++)
				{
					IntVec3 intVec2 = intVec + GenAdj.AdjacentCellsAndInside[i];
					if (intVec2.InBounds(Find.CurrentMap))
					{
						Room room2 = intVec2.GetRoom(Find.CurrentMap, RegionType.Set_All);
						if (room2 != null)
						{
							if ((!room2.PsychologicallyOutdoors && !room2.UsesOutdoorTemperature) || (!room2.PsychologicallyOutdoors && (room == null || room.PsychologicallyOutdoors)) || (room2.PsychologicallyOutdoors && room == null))
							{
								c = intVec2;
								room = room2;
							}
						}
					}
				}
			}
			if (room == null && intVec.InBounds(Find.CurrentMap))
			{
				Building edifice = intVec.GetEdifice(Find.CurrentMap);
				if (edifice != null)
				{
					CellRect.CellRectIterator iterator = edifice.OccupiedRect().ExpandedBy(1).ClipInsideMap(Find.CurrentMap).GetIterator();
					while (!iterator.Done())
					{
						IntVec3 intVec3 = iterator.Current;
						room = intVec3.GetRoom(Find.CurrentMap, RegionType.Set_All);
						if (room != null && !room.PsychologicallyOutdoors)
						{
							c = intVec3;
							break;
						}
						iterator.MoveNext();
					}
				}
			}
			string str;
			if (c.InBounds(Find.CurrentMap) && !c.Fogged(Find.CurrentMap) && room != null && !room.PsychologicallyOutdoors)
			{
				if (room.OpenRoofCount == 0)
				{
					str = "Indoors".Translate();
				}
				else
				{
					str = "IndoorsUnroofed".Translate() + " (" + room.OpenRoofCount.ToStringCached() + ")";
				}
			}
			else
			{
				str = "Outdoors".Translate();
			}
			float celsiusTemp = (room != null && !c.Fogged(Find.CurrentMap)) ? room.Temperature : Find.CurrentMap.mapTemperature.OutdoorTemp;
			return str + " " + celsiusTemp.ToStringTemperature("F0");
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x0019EE64 File Offset: 0x0019D264
		private static void DoCountdownTimer(Rect rect, TimedForcedExit timedForcedExit)
		{
			string forceExitAndRemoveMapCountdownTimeLeftString = timedForcedExit.ForceExitAndRemoveMapCountdownTimeLeftString;
			string text = "ForceExitAndRemoveMapCountdown".Translate(new object[]
			{
				forceExitAndRemoveMapCountdownTimeLeftString
			});
			float x = Text.CalcSize(text).x;
			Rect rect2 = new Rect(rect.xMax - x, rect.y, x, rect.height);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			TooltipHandler.TipRegion(rect2, "ForceExitAndRemoveMapCountdownTip".Translate(new object[]
			{
				forceExitAndRemoveMapCountdownTimeLeftString
			}));
			Widgets.Label(rect2, text);
		}
	}
}
