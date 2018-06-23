using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000984 RID: 2436
	public static class GenWorld
	{
		// Token: 0x04002370 RID: 9072
		private static int cachedTile_noSnap = -1;

		// Token: 0x04002371 RID: 9073
		private static int cachedFrame_noSnap = -1;

		// Token: 0x04002372 RID: 9074
		private static int cachedTile_snap = -1;

		// Token: 0x04002373 RID: 9075
		private static int cachedFrame_snap = -1;

		// Token: 0x04002374 RID: 9076
		public const float MaxRayLength = 1500f;

		// Token: 0x04002375 RID: 9077
		private static List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();

		// Token: 0x060036E8 RID: 14056 RVA: 0x001D5BF8 File Offset: 0x001D3FF8
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

		// Token: 0x060036E9 RID: 14057 RVA: 0x001D5C90 File Offset: 0x001D4090
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
