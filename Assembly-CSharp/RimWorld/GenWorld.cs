using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000986 RID: 2438
	public static class GenWorld
	{
		// Token: 0x04002371 RID: 9073
		private static int cachedTile_noSnap = -1;

		// Token: 0x04002372 RID: 9074
		private static int cachedFrame_noSnap = -1;

		// Token: 0x04002373 RID: 9075
		private static int cachedTile_snap = -1;

		// Token: 0x04002374 RID: 9076
		private static int cachedFrame_snap = -1;

		// Token: 0x04002375 RID: 9077
		public const float MaxRayLength = 1500f;

		// Token: 0x04002376 RID: 9078
		private static List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();

		// Token: 0x060036EC RID: 14060 RVA: 0x001D5D38 File Offset: 0x001D4138
		public static int MouseTile(bool snapToExpandableWorldObjects = false)
		{
			int result;
			if (snapToExpandableWorldObjects)
			{
				if (GenWorld.cachedFrame_snap == Time.frameCount)
				{
					result = GenWorld.cachedTile_snap;
				}
				else
				{
					GenWorld.cachedTile_snap = GenWorld.TileAt(UI.MousePositionOnUI, true);
					GenWorld.cachedFrame_snap = Time.frameCount;
					result = GenWorld.cachedTile_snap;
				}
			}
			else if (GenWorld.cachedFrame_noSnap == Time.frameCount)
			{
				result = GenWorld.cachedTile_noSnap;
			}
			else
			{
				GenWorld.cachedTile_noSnap = GenWorld.TileAt(UI.MousePositionOnUI, false);
				GenWorld.cachedFrame_noSnap = Time.frameCount;
				result = GenWorld.cachedTile_noSnap;
			}
			return result;
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x001D5DD0 File Offset: 0x001D41D0
		public static int TileAt(Vector2 clickPos, bool snapToExpandableWorldObjects = false)
		{
			Camera worldCamera = Find.WorldCamera;
			int result;
			if (!worldCamera.gameObject.activeInHierarchy)
			{
				result = -1;
			}
			else
			{
				if (snapToExpandableWorldObjects)
				{
					ExpandableWorldObjectsUtility.GetExpandedWorldObjectUnderMouse(UI.MousePositionOnUI, GenWorld.tmpWorldObjectsUnderMouse);
					if (GenWorld.tmpWorldObjectsUnderMouse.Any<WorldObject>())
					{
						int tile = GenWorld.tmpWorldObjectsUnderMouse[0].Tile;
						GenWorld.tmpWorldObjectsUnderMouse.Clear();
						return tile;
					}
				}
				Ray ray = worldCamera.ScreenPointToRay(clickPos * Prefs.UIScale);
				int worldLayerMask = WorldCameraManager.WorldLayerMask;
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 1500f, worldLayerMask))
				{
					result = Find.World.renderer.GetTileIDFromRayHit(hit);
				}
				else
				{
					result = -1;
				}
			}
			return result;
		}
	}
}
