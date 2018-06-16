using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E1 RID: 2273
	public static class CaravanNeedsTabUtility
	{
		// Token: 0x0600340F RID: 13327 RVA: 0x001BD0DC File Offset: 0x001BB4DC
		public static void DoRows(Vector2 size, List<Pawn> pawns, Caravan caravan, ref Vector2 scrollPosition, ref float scrollViewHeight, ref Pawn specificNeedsTabForPawn, bool doNeeds = true)
		{
			if (specificNeedsTabForPawn != null && (!pawns.Contains(specificNeedsTabForPawn) || specificNeedsTabForPawn.Dead))
			{
				specificNeedsTabForPawn = null;
			}
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, size.x, size.y).ContractedBy(10f);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, scrollViewHeight);
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			float num = 0f;
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref num, viewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					CaravanNeedsTabUtility.DoRow(ref num, viewRect, rect, scrollPosition, pawn, caravan, ref specificNeedsTabForPawn, doNeeds);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn2 = pawns[j];
				if (!pawn2.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref num, viewRect.width, "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					CaravanNeedsTabUtility.DoRow(ref num, viewRect, rect, scrollPosition, pawn2, caravan, ref specificNeedsTabForPawn, doNeeds);
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x001BD274 File Offset: 0x001BB674
		public static Vector2 GetSize(List<Pawn> pawns, float paneTopY, bool doNeeds = true)
		{
			float num = 100f;
			if (doNeeds)
			{
				num += (float)CaravanNeedsTabUtility.MaxNeedsCount(pawns) * 100f;
			}
			num += 24f;
			Vector2 result;
			result.x = 103f + num + 16f;
			result.y = Mathf.Min(550f, paneTopY - 30f);
			return result;
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x001BD2DC File Offset: 0x001BB6DC
		private static int MaxNeedsCount(List<Pawn> pawns)
		{
			int num = 0;
			for (int i = 0; i < pawns.Count; i++)
			{
				CaravanNeedsTabUtility.GetNeedsToDisplay(pawns[i], CaravanNeedsTabUtility.tmpNeeds);
				num = Mathf.Max(num, CaravanNeedsTabUtility.tmpNeeds.Count);
			}
			return num;
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x001BD330 File Offset: 0x001BB730
		private static void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Vector2 scrollPosition, Pawn pawn, Caravan caravan, ref Pawn specificNeedsTabForPawn, bool doNeeds)
		{
			float num = scrollPosition.y - 50f;
			float num2 = scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				CaravanNeedsTabUtility.DoRow(new Rect(0f, curY, viewRect.width, 50f), pawn, caravan, ref specificNeedsTabForPawn, doNeeds);
			}
			curY += 50f;
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x001BD39C File Offset: 0x001BB79C
		private static void DoRow(Rect rect, Pawn pawn, Caravan caravan, ref Pawn specificNeedsTabForPawn, bool doNeeds)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, pawn, caravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, pawn);
			rect2.width -= 24f;
			if (!pawn.Dead)
			{
				CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, pawn, ref specificNeedsTabForPawn);
				rect2.width -= 24f;
			}
			Widgets.DrawHighlightIfMouseover(rect2);
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, pawn, 1f);
			Rect bgRect = new Rect(rect3.xMax + 4f, 16f, 100f, 18f);
			GenMapUI.DrawPawnLabel(pawn, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			if (doNeeds)
			{
				CaravanNeedsTabUtility.GetNeedsToDisplay(pawn, CaravanNeedsTabUtility.tmpNeeds);
				float xMax = bgRect.xMax;
				for (int i = 0; i < CaravanNeedsTabUtility.tmpNeeds.Count; i++)
				{
					Need need = CaravanNeedsTabUtility.tmpNeeds[i];
					int maxThresholdMarkers = 0;
					bool doTooltip = true;
					Rect rect4 = new Rect(xMax, 0f, 100f, 50f);
					Need_Mood mood = need as Need_Mood;
					if (mood != null)
					{
						maxThresholdMarkers = 1;
						doTooltip = false;
						TooltipHandler.TipRegion(rect4, new TipSignal(() => CaravanNeedsTabUtility.CustomMoodNeedTooltip(mood), rect4.GetHashCode()));
					}
					need.DrawOnGUI(rect4, maxThresholdMarkers, 10f, false, doTooltip);
					xMax = rect4.xMax;
				}
			}
			if (pawn.Downed)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			GUI.EndGroup();
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x001BD5D8 File Offset: 0x001BB9D8
		private static void GetNeedsToDisplay(Pawn p, List<Need> outNeeds)
		{
			outNeeds.Clear();
			List<Need> allNeeds = p.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				Need need = allNeeds[i];
				if (need.def.showForCaravanMembers)
				{
					outNeeds.Add(need);
				}
			}
			PawnNeedsUIUtility.SortInDisplayOrder(outNeeds);
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x001BD63C File Offset: 0x001BBA3C
		private static string CustomMoodNeedTooltip(Need_Mood mood)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(mood.GetTipString());
			PawnNeedsUIUtility.GetThoughtGroupsInDisplayOrder(mood, CaravanNeedsTabUtility.thoughtGroupsPresent);
			bool flag = false;
			for (int i = 0; i < CaravanNeedsTabUtility.thoughtGroupsPresent.Count; i++)
			{
				Thought group = CaravanNeedsTabUtility.thoughtGroupsPresent[i];
				mood.thoughts.GetMoodThoughts(group, CaravanNeedsTabUtility.thoughtGroup);
				Thought leadingThoughtInGroup = PawnNeedsUIUtility.GetLeadingThoughtInGroup(CaravanNeedsTabUtility.thoughtGroup);
				if (leadingThoughtInGroup.VisibleInNeedsTab)
				{
					if (!flag)
					{
						flag = true;
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(leadingThoughtInGroup.LabelCap);
					if (CaravanNeedsTabUtility.thoughtGroup.Count > 1)
					{
						stringBuilder.Append(" x");
						stringBuilder.Append(CaravanNeedsTabUtility.thoughtGroup.Count);
					}
					stringBuilder.Append(": ");
					stringBuilder.AppendLine(mood.thoughts.MoodOffsetOfGroup(group).ToString("##0"));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001C14 RID: 7188
		private const float RowHeight = 50f;

		// Token: 0x04001C15 RID: 7189
		private const float LabelHeight = 18f;

		// Token: 0x04001C16 RID: 7190
		private const float LabelColumnWidth = 100f;

		// Token: 0x04001C17 RID: 7191
		private const float NeedWidth = 100f;

		// Token: 0x04001C18 RID: 7192
		private const float NeedMargin = 10f;

		// Token: 0x04001C19 RID: 7193
		private static List<Need> tmpNeeds = new List<Need>();

		// Token: 0x04001C1A RID: 7194
		private static List<Thought> thoughtGroupsPresent = new List<Thought>();

		// Token: 0x04001C1B RID: 7195
		private static List<Thought> thoughtGroup = new List<Thought>();
	}
}
