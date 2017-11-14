using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	internal static class EnvironmentInspectDrawer
	{
		private const float StatLabelColumnWidth = 100f;

		private const float ScoreColumnWidth = 50f;

		private const float ScoreStageLabelColumnWidth = 160f;

		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		private const float DistFromMouse = 26f;

		private const float WindowPadding = 18f;

		private const float LineHeight = 23f;

		private const float SpaceBetweenLines = 2f;

		private const float SpaceBetweenColumns = 35f;

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

		private static bool ShouldShow()
		{
			if (!Find.PlaySettings.showEnvironment)
			{
				return false;
			}
			if (Mouse.IsInputBlockedNow)
			{
				return false;
			}
			if (UI.MouseCell().InBounds(Find.VisibleMap) && !UI.MouseCell().Fogged(Find.VisibleMap))
			{
				return true;
			}
			return false;
		}

		public static void EnvironmentInspectOnGUI()
		{
			if (Event.current.type == EventType.Repaint && EnvironmentInspectDrawer.ShouldShow())
			{
				BeautyDrawer.DrawBeautyAroundMouse();
				EnvironmentInspectDrawer.DrawInfoWindow();
			}
		}

		private static void DrawInfoWindow()
		{
			Room room = UI.MouseCell().GetRoom(Find.VisibleMap, RegionType.Set_All);
			bool roomValid = room != null && room.Role != RoomRoleDefOf.None;
			Text.Font = GameFont.Small;
			Vector2 mousePosition = Event.current.mousePosition;
			float x = mousePosition.x;
			Vector2 mousePosition2 = Event.current.mousePosition;
			Rect windowRect = new Rect(x, mousePosition2.y, 416f, 36f);
			windowRect.height += 25f;
			if (roomValid)
			{
				windowRect.height += 13f;
				windowRect.height += 23f;
				windowRect.height += (float)((float)EnvironmentInspectDrawer.DisplayedRoomStatsCount * 25.0);
			}
			windowRect.x += 26f;
			windowRect.y += 26f;
			if (windowRect.xMax > (float)UI.screenWidth)
			{
				windowRect.x -= (float)(windowRect.width + 52.0);
			}
			if (windowRect.yMax > (float)UI.screenHeight)
			{
				windowRect.y -= (float)(windowRect.height + 52.0);
			}
			Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.Super, delegate
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InspectRoomStats, KnowledgeAmount.FrameDisplayed);
				Text.Font = GameFont.Small;
				float num = 18f;
				float beauty = BeautyUtility.AverageBeautyPerceptible(UI.MouseCell(), Find.VisibleMap);
				Rect rect = new Rect(18f, num, (float)(windowRect.width - 36.0), 100f);
				GUI.color = BeautyDrawer.BeautyColor(beauty, 40f);
				Widgets.Label(rect, "BeautyHere".Translate() + ": " + beauty.ToString("F1"));
				num = (float)(num + 25.0);
				if (roomValid)
				{
					num = (float)(num + 5.0);
					GUI.color = new Color(1f, 1f, 1f, 0.4f);
					Widgets.DrawLineHorizontal(18f, num, (float)(windowRect.width - 36.0));
					GUI.color = Color.white;
					num = (float)(num + 8.0);
					Rect rect2 = new Rect(18f, num, (float)(windowRect.width - 36.0), 100f);
					GUI.color = Color.white;
					Widgets.Label(rect2, EnvironmentInspectDrawer.GetRoomRoleLabel(room));
					num = (float)(num + 25.0);
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
								GUI.color = EnvironmentInspectDrawer.RelatedStatColor;
							}
							else
							{
								GUI.color = Color.gray;
							}
							Rect rect3 = new Rect(rect2.x, num, 100f, 23f);
							Widgets.Label(rect3, roomStatDef.LabelCap);
							Rect rect4 = new Rect((float)(rect3.xMax + 35.0), num, 50f, 23f);
							string label = roomStatDef.ScoreToString(stat);
							Widgets.Label(rect4, label);
							Rect rect5 = new Rect((float)(rect4.xMax + 35.0), num, 160f, 23f);
							Widgets.Label(rect5, (scoreStage != null) ? scoreStage.label : string.Empty);
							num = (float)(num + 25.0);
						}
					}
					Text.WordWrap = true;
				}
				GUI.color = Color.white;
			}, true, false, 1f);
		}

		public static void DrawRoomOverlays()
		{
			if (EnvironmentInspectDrawer.ShouldShow())
			{
				GenUI.RenderMouseoverBracket();
				Room room = UI.MouseCell().GetRoom(Find.VisibleMap, RegionType.Set_All);
				if (room != null && room.Role != RoomRoleDefOf.None)
				{
					room.DrawFieldEdges();
				}
			}
		}

		private static string GetRoomRoleLabel(Room room)
		{
			Pawn pawn = null;
			Pawn pawn2 = null;
			foreach (Pawn owner in room.Owners)
			{
				if (pawn == null)
				{
					pawn = owner;
				}
				else
				{
					pawn2 = owner;
				}
			}
			if (pawn == null)
			{
				return room.Role.LabelCap;
			}
			if (pawn2 == null)
			{
				return "SomeonesRoom".Translate(pawn.NameStringShort, room.Role.label);
			}
			return "CouplesRoom".Translate(pawn.NameStringShort, pawn2.NameStringShort, room.Role.label);
		}
	}
}
