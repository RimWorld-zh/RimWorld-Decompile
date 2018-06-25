using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200085E RID: 2142
	[StaticConstructorOnStartup]
	internal class InspectPaneFiller
	{
		// Token: 0x04001A3D RID: 6717
		private const float BarHeight = 16f;

		// Token: 0x04001A3E RID: 6718
		private static readonly Texture2D MoodTex;

		// Token: 0x04001A3F RID: 6719
		private static readonly Texture2D BarBGTex;

		// Token: 0x04001A40 RID: 6720
		private static readonly Texture2D HealthTex;

		// Token: 0x04001A41 RID: 6721
		private const float BarWidth = 93f;

		// Token: 0x04001A42 RID: 6722
		private const float BarSpacing = 6f;

		// Token: 0x04001A43 RID: 6723
		private static bool debug_inspectStringExceptionErrored;

		// Token: 0x04001A44 RID: 6724
		private static Vector2 inspectStringScrollPos;

		// Token: 0x06003087 RID: 12423 RVA: 0x001A5C14 File Offset: 0x001A4014
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
					num += 3f;
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
					num += 18f;
				}
				Rect rect2 = rect.AtZero();
				rect2.yMin = num;
				InspectPaneFiller.DrawInspectStringFor(sel, rect2);
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Error in DoPaneContentsFor ",
					Find.Selector.FirstSelectedObject,
					": ",
					ex.ToString()
				}), 754672, false);
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x001A5D28 File Offset: 0x001A4128
		public static void DrawHealth(WidgetRow row, Thing t)
		{
			Pawn pawn = t as Pawn;
			float fillPct;
			string label;
			if (pawn == null)
			{
				if (!t.def.useHitPoints)
				{
					return;
				}
				if (t.HitPoints >= t.MaxHitPoints)
				{
					GUI.color = Color.white;
				}
				else if ((float)t.HitPoints > (float)t.MaxHitPoints * 0.5f)
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
			}
			else
			{
				GUI.color = Color.white;
				fillPct = pawn.health.summaryHealth.SummaryHealthPercent;
				label = HealthUtility.GetGeneralConditionLabel(pawn, true);
			}
			row.FillableBar(93f, 16f, fillPct, label, InspectPaneFiller.HealthTex, InspectPaneFiller.BarBGTex);
			GUI.color = Color.white;
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x001A5E4C File Offset: 0x001A424C
		private static void DrawMood(WidgetRow row, Pawn pawn)
		{
			if (pawn.needs != null && pawn.needs.mood != null)
			{
				row.Gap(6f);
				row.FillableBar(93f, 16f, pawn.needs.mood.CurLevelPercentage, pawn.needs.mood.MoodString.CapitalizeFirst(), InspectPaneFiller.MoodTex, InspectPaneFiller.BarBGTex);
			}
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x001A5EC8 File Offset: 0x001A42C8
		private static void DrawTimetableSetting(WidgetRow row, Pawn pawn)
		{
			row.Gap(6f);
			row.FillableBar(93f, 16f, 1f, pawn.timetable.CurrentAssignment.LabelCap, pawn.timetable.CurrentAssignment.ColorTexture, null);
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x001A5F18 File Offset: 0x001A4318
		private static void DrawAreaAllowed(WidgetRow row, Pawn pawn)
		{
			if (pawn.playerSettings != null && pawn.playerSettings.RespectsAllowedArea)
			{
				row.Gap(6f);
				bool flag = pawn.playerSettings != null && pawn.playerSettings.AreaRestriction != null;
				Texture2D fillTex;
				if (flag)
				{
					fillTex = pawn.playerSettings.AreaRestriction.ColorTexture;
				}
				else
				{
					fillTex = BaseContent.GreyTex;
				}
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
					AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
					{
						pawn.playerSettings.AreaRestriction = a;
					}, true, true, pawn.Map);
				}
			}
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x001A6048 File Offset: 0x001A4448
		public static void DrawInspectStringFor(ISelectable sel, Rect rect)
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
					Log.Error(text, false);
					InspectPaneFiller.debug_inspectStringExceptionErrored = true;
				}
			}
			if (!text.NullOrEmpty() && GenText.ContainsEmptyLines(text))
			{
				Log.ErrorOnce(string.Format("Inspect string for {0} contains empty lines.\n\nSTART\n{1}\nEND", sel, text), 837163521, false);
			}
			InspectPaneFiller.DrawInspectString(text, rect);
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x001A612C File Offset: 0x001A452C
		public static void DrawInspectString(string str, Rect rect)
		{
			Text.Font = GameFont.Small;
			Widgets.LabelScrollable(rect, str, ref InspectPaneFiller.inspectStringScrollPos, true);
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x001A6144 File Offset: 0x001A4544
		// Note: this type is marked as 'beforefieldinit'.
		static InspectPaneFiller()
		{
			ColorInt colorInt = new ColorInt(26, 52, 52);
			InspectPaneFiller.MoodTex = SolidColorMaterials.NewSolidColorTexture(colorInt.ToColor);
			ColorInt colorInt2 = new ColorInt(10, 10, 10);
			InspectPaneFiller.BarBGTex = SolidColorMaterials.NewSolidColorTexture(colorInt2.ToColor);
			ColorInt colorInt3 = new ColorInt(35, 35, 35);
			InspectPaneFiller.HealthTex = SolidColorMaterials.NewSolidColorTexture(colorInt3.ToColor);
			InspectPaneFiller.debug_inspectStringExceptionErrored = false;
		}
	}
}
