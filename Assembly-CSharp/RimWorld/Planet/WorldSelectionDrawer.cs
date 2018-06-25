using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008F0 RID: 2288
	public static class WorldSelectionDrawer
	{
		// Token: 0x04001C8B RID: 7307
		private static Dictionary<WorldObject, float> selectTimes = new Dictionary<WorldObject, float>();

		// Token: 0x04001C8C RID: 7308
		private const float BaseSelectedTexJump = 25f;

		// Token: 0x04001C8D RID: 7309
		private const float BaseSelectedTexScale = 0.4f;

		// Token: 0x04001C8E RID: 7310
		private const float BaseSelectionRectSize = 35f;

		// Token: 0x04001C8F RID: 7311
		private static readonly Color HiddenSelectionBracketColor = new Color(1f, 1f, 1f, 0.35f);

		// Token: 0x04001C90 RID: 7312
		private static Vector2[] bracketLocs = new Vector2[4];

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060034C9 RID: 13513 RVA: 0x001C3B34 File Offset: 0x001C1F34
		public static Dictionary<WorldObject, float> SelectTimes
		{
			get
			{
				return WorldSelectionDrawer.selectTimes;
			}
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x001C3B4E File Offset: 0x001C1F4E
		public static void Notify_Selected(WorldObject t)
		{
			WorldSelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x001C3B61 File Offset: 0x001C1F61
		public static void Clear()
		{
			WorldSelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x001C3B70 File Offset: 0x001C1F70
		public static void SelectionOverlaysOnGUI()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				WorldObject worldObject = selectedObjects[i];
				WorldSelectionDrawer.DrawSelectionBracketOnGUIFor(worldObject);
				worldObject.ExtraSelectionOverlaysOnGUI();
			}
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x001C3BB8 File Offset: 0x001C1FB8
		public static void DrawSelectionOverlays()
		{
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				WorldObject worldObject = selectedObjects[i];
				worldObject.DrawExtraSelectionOverlays();
			}
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x001C3BF8 File Offset: 0x001C1FF8
		private static void DrawSelectionBracketOnGUIFor(WorldObject obj)
		{
			Vector2 vector = obj.ScreenPos();
			Rect rect = new Rect(vector.x - 17.5f, vector.y - 17.5f, 35f, 35f);
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * 0.4f, (float)SelectionDrawerUtility.SelectedTexGUI.height * 0.4f);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<WorldObject>(WorldSelectionDrawer.bracketLocs, obj, rect, WorldSelectionDrawer.selectTimes, textureSize, 25f);
			if (obj.HiddenBehindTerrainNow())
			{
				GUI.color = WorldSelectionDrawer.HiddenSelectionBracketColor;
			}
			else
			{
				GUI.color = Color.white;
			}
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(WorldSelectionDrawer.bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, 0.4f);
				num += 90;
			}
			GUI.color = Color.white;
		}
	}
}
