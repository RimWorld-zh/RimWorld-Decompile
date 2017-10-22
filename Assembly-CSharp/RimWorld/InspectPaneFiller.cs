using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	internal class InspectPaneFiller
	{
		private const float BarHeight = 16f;

		private const float BarWidth = 93f;

		private const float BarSpacing = 6f;

		private static readonly Texture2D MoodTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(26, 52, 52).ToColor);

		private static readonly Texture2D BarBGTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(10, 10, 10).ToColor);

		private static readonly Texture2D HealthTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(35, 35, 35).ToColor);

		private static bool debug_inspectLengthWarned = false;

		private static bool debug_inspectStringExceptionErrored = false;

		public static void DoPaneContentsFor(ISelectable sel, Rect rect)
		{
			try
			{
				GUI.BeginGroup(rect);
				float num = 0f;
				Thing thing = sel as Thing;
				Pawn pawn = sel as Pawn;
				if (thing != null)
				{
					num = (float)(num + 3.0);
					WidgetRow row = new WidgetRow(0f, num, UIDirection.RightThenUp, 99999f, 4f);
					InspectPaneFiller.DrawHealth(row, thing);
					if (pawn != null)
					{
						InspectPaneFiller.DrawMood(row, pawn);
						if (pawn.timetable != null)
						{
							InspectPaneFiller.DrawTimetableSetting(row, pawn);
						}
						InspectPaneFiller.DrawAreaAllowed(row, pawn);
					}
					num = (float)(num + 18.0);
				}
				InspectPaneFiller.DrawInspectStringFor(sel, ref num);
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Error in DoPaneContentsFor " + Find.Selector.FirstSelectedObject + ": " + ex.ToString(), 754672);
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		public static void DrawHealth(WidgetRow row, Thing t)
		{
			Pawn pawn = t as Pawn;
			float fillPct;
			string label;
			if (pawn == null)
			{
				if (t.def.useHitPoints)
				{
					if (t.HitPoints >= t.MaxHitPoints)
					{
						GUI.color = Color.white;
					}
					else if ((float)t.HitPoints > (float)t.MaxHitPoints * 0.5)
					{
						GUI.color = Color.yellow;
					}
					else if (t.HitPoints > 0)
					{
						GUI.color = Color.red;
					}
					else
					{
						GUI.color = Color.grey;
					}
					fillPct = (float)t.HitPoints / (float)t.MaxHitPoints;
					label = t.HitPoints.ToStringCached() + " / " + t.MaxHitPoints.ToStringCached();
					goto IL_00e4;
				}
				return;
			}
			GUI.color = Color.white;
			fillPct = pawn.health.summaryHealth.SummaryHealthPercent;
			label = HealthUtility.GetGeneralConditionLabel(pawn, true);
			goto IL_00e4;
			IL_00e4:
			row.FillableBar(93f, 16f, fillPct, label, InspectPaneFiller.HealthTex, InspectPaneFiller.BarBGTex);
			GUI.color = Color.white;
		}

		private static void DrawMood(WidgetRow row, Pawn pawn)
		{
			if (pawn.needs != null && pawn.needs.mood != null)
			{
				row.Gap(6f);
				row.FillableBar(93f, 16f, pawn.needs.mood.CurLevelPercentage, pawn.needs.mood.MoodString.CapitalizeFirst(), InspectPaneFiller.MoodTex, InspectPaneFiller.BarBGTex);
			}
		}

		private static void DrawTimetableSetting(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, 1f, pawn.timetable.CurrentAssignment.LabelCap, pawn.timetable.CurrentAssignment.ColorTexture, null);
		}

		private static void DrawAreaAllowed(WidgetRow row, Pawn pawn)
		{
			if (pawn.playerSettings != null && pawn.playerSettings.RespectsAllowedArea)
			{
				row.Gap(6f);
				bool flag = pawn.playerSettings != null && pawn.playerSettings.AreaRestriction != null;
				Texture2D fillTex = (!flag) ? BaseContent.GreyTex : pawn.playerSettings.AreaRestriction.ColorTexture;
				Rect rect = row.FillableBar(93f, 16f, 1f, AreaUtility.AreaAllowedLabel(pawn), fillTex, null);
				if (Mouse.IsOver(rect))
				{
					if (flag)
					{
						pawn.playerSettings.AreaRestriction.MarkForDraw();
					}
					Rect rect2 = rect.ContractedBy(-1f);
					Widgets.DrawBox(rect2, 1);
				}
				if (Widgets.ButtonInvisible(rect, false))
				{
					AllowedAreaMode mode = (AllowedAreaMode)(pawn.RaceProps.Humanlike ? 1 : 2);
					AreaUtility.MakeAllowedAreaListFloatMenu((Action<Area>)delegate(Area a)
					{
						pawn.playerSettings.AreaRestriction = a;
					}, mode, true, true, pawn.Map);
				}
			}
		}

		public static void DrawInspectStringFor(ISelectable sel, ref float y)
		{
			string text;
			try
			{
				text = sel.GetInspectString();
				Thing thing = sel as Thing;
				if (thing != null)
				{
					string inspectStringLowPriority = thing.GetInspectStringLowPriority();
					if (!inspectStringLowPriority.NullOrEmpty())
					{
						if (!text.NullOrEmpty())
						{
							text = text.TrimEndNewlines() + "\n";
						}
						text += inspectStringLowPriority;
					}
				}
			}
			catch (Exception ex)
			{
				text = "GetInspectString exception on " + sel.ToString() + ":\n" + ex.ToString();
				if (!InspectPaneFiller.debug_inspectStringExceptionErrored)
				{
					Log.Error(text);
					InspectPaneFiller.debug_inspectStringExceptionErrored = true;
				}
			}
			if (!text.NullOrEmpty() && GenText.ContainsEmptyLines(text))
			{
				Log.ErrorOnce("Inspect string for " + sel + " contains empty lines.", 837163521);
			}
			InspectPaneFiller.DrawInspectString(text, ref y);
			if (Prefs.DevMode)
			{
				text = text.Trim();
				if (!InspectPaneFiller.debug_inspectLengthWarned && text.Count((Func<char, bool>)((char f) => f == '\n')) > 5)
				{
					Log.ErrorOnce(sel + " gave an inspect string over six lines (some may be empty):\n" + text + "END", 778772);
					InspectPaneFiller.debug_inspectLengthWarned = true;
				}
			}
		}

		public static void DrawInspectString(string str, ref float y)
		{
			Text.Font = GameFont.Small;
			float y2 = y;
			Vector2 paneInnerSize = InspectPaneUtility.PaneInnerSize;
			Rect rect = new Rect(0f, y2, paneInnerSize.x, 200f);
			Widgets.Label(rect, str);
		}
	}
}
