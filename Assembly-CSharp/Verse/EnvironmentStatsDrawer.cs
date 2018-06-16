using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E5F RID: 3679
	internal static class EnvironmentStatsDrawer
	{
		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06005692 RID: 22162 RVA: 0x002C93D0 File Offset: 0x002C77D0
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

		// Token: 0x06005693 RID: 22163 RVA: 0x002C9428 File Offset: 0x002C7828
		private static bool ShouldShowWindowNow()
		{
			return (EnvironmentStatsDrawer.ShouldShowRoomStats() || EnvironmentStatsDrawer.ShouldShowBeauty()) && !Mouse.IsInputBlockedNow;
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x002C946C File Offset: 0x002C786C
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

		// Token: 0x06005695 RID: 22165 RVA: 0x002C94F0 File Offset: 0x002C78F0
		private static bool ShouldShowBeauty()
		{
			return Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && UI.MouseCell().GetRoom(Find.CurrentMap, RegionType.Set_Passable) != null;
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x002C9560 File Offset: 0x002C7960
		public static void EnvironmentStatsOnGUI()
		{
			if (Event.current.type == EventType.Repaint && EnvironmentStatsDrawer.ShouldShowWindowNow())
			{
				EnvironmentStatsDrawer.DrawInfoWindow();
			}
		}

		// Token: 0x06005697 RID: 22167 RVA: 0x002C9588 File Offset: 0x002C7988
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

		// Token: 0x06005698 RID: 22168 RVA: 0x002C9728 File Offset: 0x002C7B28
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

		// Token: 0x06005699 RID: 22169 RVA: 0x002C99C0 File Offset: 0x002C7DC0
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

		// Token: 0x0600569A RID: 22170 RVA: 0x002C9A3C File Offset: 0x002C7E3C
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

		// Token: 0x0400395A RID: 14682
		private const float StatLabelColumnWidth = 100f;

		// Token: 0x0400395B RID: 14683
		private const float ScoreColumnWidth = 50f;

		// Token: 0x0400395C RID: 14684
		private const float ScoreStageLabelColumnWidth = 160f;

		// Token: 0x0400395D RID: 14685
		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		// Token: 0x0400395E RID: 14686
		private const float DistFromMouse = 26f;

		// Token: 0x0400395F RID: 14687
		private const float WindowPadding = 18f;

		// Token: 0x04003960 RID: 14688
		private const float LineHeight = 23f;

		// Token: 0x04003961 RID: 14689
		private const float SpaceBetweenLines = 2f;

		// Token: 0x04003962 RID: 14690
		private const float SpaceBetweenColumns = 35f;
	}
}
