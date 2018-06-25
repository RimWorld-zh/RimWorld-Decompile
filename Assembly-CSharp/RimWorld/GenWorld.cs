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
		// Token: 0x04002378 RID: 9080
		private static int cachedTile_noSnap = -1;

		// Token: 0x04002379 RID: 9081
		private static int cachedFrame_noSnap = -1;

		// Token: 0x0400237A RID: 9082
		private static int cachedTile_snap = -1;

		// Token: 0x0400237B RID: 9083
		private static int cachedFrame_snap = -1;

		// Token: 0x0400237C RID: 9084
		public const float MaxRayLength = 1500f;

		// Token: 0x0400237D RID: 9085
		private static List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();

		// Token: 0x060036EC RID: 14060 RVA: 0x001D600C File Offset: 0x001D440C
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

		// Token: 0x060036ED RID: 14061 RVA: 0x001D60A4 File Offset: 0x001D44A4
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
