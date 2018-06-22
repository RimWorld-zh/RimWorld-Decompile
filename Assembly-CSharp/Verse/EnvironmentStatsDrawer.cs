using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5D RID: 3677
	internal static class EnvironmentStatsDrawer
	{
		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x060056B0 RID: 22192 RVA: 0x002CAFE0 File Offset: 0x002C93E0
		private static int DisplayedRoomStatsCount
		{
			get
			{
				int num = 0;
				List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!allDefsListForReading[i].isHidden || DebugViewSettings.showAllRoomStats)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x002CB038 File Offset: 0x002C9438
		private static bool ShouldShowWindowNow()
		{
			return (EnvironmentStatsDrawer.ShouldShowRoomStats() || EnvironmentStatsDrawer.ShouldShowBeauty()) && !Mouse.IsInputBlockedNow;
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x002CB07C File Offset: 0x002C947C
		private static bool ShouldShowRoomStats()
		{
			bool result;
			if (!Find.PlaySettings.showRoomStats)
			{
				result = false;
			}
			else if (!UI.MouseCell().InBounds(Find.CurrentMap) || UI.MouseCell().Fogged(Find.CurrentMap))
			{
				result = false;
			}
			else
			{
				Room room = UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_All);
				result = (room != null && room.Role != RoomRoleDefOf.None);
			}
			return result;
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x002CB100 File Offset: 0x002C9500
		private static bool ShouldShowBeauty()
		{
			return Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_Passable) != null;
		}

		// Token: 0x060056B4 RID: 22196 RVA: 0x002CB170 File Offset: 0x002C9570
		public static void EnvironmentStatsOnGUI()
		{
			if (Event.current.type == EventType.Repaint && EnvironmentStatsDrawer.ShouldShowWindowNow())
			{
				EnvironmentStatsDrawer.DrawInfoWindow();
			}
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x002CB198 File Offset: 0x002C9598
		private static void DrawInfoWindow()
		{
			EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0 <DrawInfoWindow>c__AnonStorey = new EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0();
			Text.Font = GameFont.Small;
			<DrawInfoWindow>c__AnonStorey.windowRect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 416f, 36f);
			bool flag = EnvironmentStatsDrawer.ShouldShowBeauty();
			if (flag)
			{
				EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0 <DrawInfoWindow>c__AnonStorey2 = <DrawInfoWindow>c__AnonStorey;
				<DrawInfoWindow>c__AnonStorey2.windowRect.height = <DrawInfoWindow>c__AnonStorey2.windowRect.height + 25f;
			}
			if (EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				if (flag)
				{
					EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0 <DrawInfoWindow>c__AnonStorey3 = <DrawInfoWindow>c__AnonStorey;
					<DrawInfoWindow>c__AnonStorey3.windowRect.height = <DrawInfoWindow>c__AnonStorey3.windowRect.height + 13f;
				}
				EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0 <DrawInfoWindow>c__AnonStorey4 = <DrawInfoWindow>c__AnonStorey;
				<DrawInfoWindow>c__AnonStorey4.windowRect.height = <DrawInfoWindow>c__AnonStorey4.windowRect.height + 23f;
				EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0 <DrawInfoWindow>c__AnonStorey5 = <DrawInfoWindow>c__AnonStorey;
				<DrawInfoWindow>c__AnonStorey5.windowRect.height = <DrawInfoWindow>c__AnonStorey5.windowRect.height + (float)EnvironmentStatsDrawer.DisplayedRoomStatsCount * 25f;
			}
			EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0 <DrawInfoWindow>c__AnonStorey6 = <DrawInfoWindow>c__AnonStorey;
			<DrawInfoWindow>c__AnonStorey6.windowRect.x = <DrawInfoWindow>c__AnonStorey6.windowRect.x + 26f;
			EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0 <DrawInfoWindow>c__AnonStorey7 = <DrawInfoWindow>c__AnonStorey;
			<DrawInfoWindow>c__AnonStorey7.windowRect.y = <DrawInfoWindow>c__AnonStorey7.windowRect.y + 26f;
			if (<DrawInfoWindow>c__AnonStorey.windowRect.xMax > (float)UI.screenWidth)
			{
				EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0 <DrawInfoWindow>c__AnonStorey8 = <DrawInfoWindow>c__AnonStorey;
				<DrawInfoWindow>c__AnonStorey8.windowRect.x = <DrawInfoWindow>c__AnonStorey8.windowRect.x - (<DrawInfoWindow>c__AnonStorey.windowRect.width + 52f);
			}
			if (<DrawInfoWindow>c__AnonStorey.windowRect.yMax > (float)UI.screenHeight)
			{
				EnvironmentStatsDrawer.<DrawInfoWindow>c__AnonStorey0 <DrawInfoWindow>c__AnonStorey9 = <DrawInfoWindow>c__AnonStorey;
				<DrawInfoWindow>c__AnonStorey9.windowRect.y = <DrawInfoWindow>c__AnonStorey9.windowRect.y - (<DrawInfoWindow>c__AnonStorey.windowRect.height + 52f);
			}
			Find.WindowStack.ImmediateWindow(74975, <DrawInfoWindow>c__AnonStorey.windowRect, WindowLayer.Super, delegate
			{
				EnvironmentStatsDrawer.FillWindow(<DrawInfoWindow>c__AnonStorey.windowRect);
			}, true, false, 1f);
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x002CB338 File Offset: 0x002C9738
		private static void FillWindow(Rect windowRect)
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InspectRoomStats, KnowledgeAmount.FrameDisplayed);
			Text.Font = GameFont.Small;
			float num = 18f;
			bool flag = EnvironmentStatsDrawer.ShouldShowBeauty();
			if (flag)
			{
				float beauty = BeautyUtility.AverageBeautyPerceptible(UI.MouseCell(), Find.CurrentMap);
				Rect rect = new Rect(18f, num, windowRect.width - 36f, 100f);
				GUI.color = BeautyDrawer.BeautyColor(beauty, 40f);
				Widgets.Label(rect, "BeautyHere".Translate() + ": " + beauty.ToString("F1"));
				num += 25f;
			}
			if (EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				if (flag)
				{
					num += 5f;
					GUI.color = new Color(1f, 1f, 1f, 0.4f);
					Widgets.DrawLineHorizontal(18f, num, windowRect.width - 36f);
					GUI.color = Color.white;
					num += 8f;
				}
				Room room = UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_All);
				Rect rect2 = new Rect(18f, num, windowRect.width - 36f, 100f);
				GUI.color = Color.white;
				Widgets.Label(rect2, EnvironmentStatsDrawer.GetRoomRoleLabel(room));
				num += 25f;
				Text.WordWrap = false;
				for (int i = 0; i < DefDatabase<RoomStatDef>.AllDefsListForReading.Count; i++)
				{
					RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.AllDefsListForReading[i];
					if (!roomStatDef.isHidden || DebugViewSettings.showAllRoomStats)
					{
						float stat = room.GetStat(roomStatDef);
						RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
						if (room.Role.IsStatRelated(roomStatDef))
						{
							GUI.color = EnvironmentStatsDrawer.RelatedStatColor;
						}
						else
						{
							GUI.color = Color.gray;
						}
						Rect rect3 = new Rect(rect2.x, num, 100f, 23f);
						Widgets.Label(rect3, roomStatDef.LabelCap);
						Rect rect4 = new Rect(rect3.xMax + 35f, num, 50f, 23f);
						string label = roomStatDef.ScoreToString(stat);
						Widgets.Label(rect4, label);
						Rect rect5 = new Rect(rect4.xMax + 35f, num, 160f, 23f);
						Widgets.Label(rect5, (scoreStage != null) ? scoreStage.label : "");
						num += 25f;
					}
				}
				Text.WordWrap = true;
			}
			GUI.color = Color.white;
		}

		// Token: 0x060056B7 RID: 22199 RVA: 0x002CB5D0 File Offset: 0x002C99D0
		public static void DrawRoomOverlays()
		{
			if (Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap))
			{
				GenUI.RenderMouseoverBracket();
			}
			if (EnvironmentStatsDrawer.ShouldShowWindowNow() && EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				Room room = UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_All);
				if (room != null && room.Role != RoomRoleDefOf.None)
				{
					room.DrawFieldEdges();
				}
			}
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x002CB64C File Offset: 0x002C9A4C
		private static string GetRoomRoleLabel(Room room)
		{
			Pawn pawn = null;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in room.Owners)
			{
				if (pawn == null)
				{
					pawn = pawn3;
				}
				else
				{
					pawn2 = pawn3;
				}
			}
			string result;
			if (pawn == null)
			{
				result = room.Role.LabelCap;
			}
			else if (pawn2 == null)
			{
				result = "SomeonesRoom".Translate(new object[]
				{
					pawn.LabelShort,
					room.Role.label
				});
			}
			else
			{
				result = "CouplesRoom".Translate(new object[]
				{
					pawn.LabelShort,
					pawn2.LabelShort,
					room.Role.label
				});
			}
			return result;
		}

		// Token: 0x04003967 RID: 14695
		private const float StatLabelColumnWidth = 100f;

		// Token: 0x04003968 RID: 14696
		private const float ScoreColumnWidth = 50f;

		// Token: 0x04003969 RID: 14697
		private const float ScoreStageLabelColumnWidth = 160f;

		// Token: 0x0400396A RID: 14698
		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		// Token: 0x0400396B RID: 14699
		private const float DistFromMouse = 26f;

		// Token: 0x0400396C RID: 14700
		private const float WindowPadding = 18f;

		// Token: 0x0400396D RID: 14701
		private const float LineHeight = 23f;

		// Token: 0x0400396E RID: 14702
		private const float SpaceBetweenLines = 2f;

		// Token: 0x0400396F RID: 14703
		private const float SpaceBetweenColumns = 35f;
	}
}
