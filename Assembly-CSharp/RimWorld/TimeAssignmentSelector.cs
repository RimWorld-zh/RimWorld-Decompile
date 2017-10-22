using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public static class TimeAssignmentSelector
	{
		public static TimeAssignmentDef selectedAssignment = TimeAssignmentDefOf.Work;

		public static void DrawTimeAssignmentSelectorGrid(Rect rect)
		{
			rect.yMax -= 2f;
			Rect rect2 = rect;
			Vector2 center = rect2.center;
			rect2.xMax = center.x;
			Vector2 center2 = rect2.center;
			rect2.yMax = center2.y;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Anything);
			rect2.x += rect2.width;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Work);
			rect2.y += rect2.height;
			rect2.x -= rect2.width;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Joy);
			rect2.x += rect2.width;
			TimeAssignmentSelector.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Sleep);
		}

		private static void DrawTimeAssignmentSelectorFor(Rect rect, TimeAssignmentDef ta)
		{
			rect = rect.ContractedBy(2f);
			GUI.DrawTexture(rect, ta.ColorTexture);
			if (Widgets.ButtonInvisible(rect, false))
			{
				TimeAssignmentSelector.selectedAssignment = ta;
				SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
			}
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = Color.white;
			Widgets.Label(rect, ta.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (TimeAssignmentSelector.selectedAssignment == ta)
			{
				Widgets.DrawBox(rect, 2);
			}
		}
	}
}
