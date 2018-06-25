using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenWorld
	{
		private static int cachedTile_noSnap = -1;

		private static int cachedFrame_noSnap = -1;

		private static int cachedTile_snap = -1;

		private static int cachedFrame_snap = -1;

		public const float MaxRayLength = 1500f;

		private static List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();

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

		// Note: this type is marked as 'beforefieldinit'.
		static GenWorld()
		{
		}
	}
}
