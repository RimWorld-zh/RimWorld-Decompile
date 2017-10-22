using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenWorld
	{
		private static int cachedTile = -1;

		private static int cachedFrame = -1;

		public const float MaxRayLength = 1500f;

		private static List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();

		public static int MouseTile(bool snapToExpandableWorldObjects = false)
		{
			int result;
			if (GenWorld.cachedFrame == Time.frameCount)
			{
				result = GenWorld.cachedTile;
			}
			else
			{
				GenWorld.cachedTile = GenWorld.TileAt(UI.MousePositionOnUI, snapToExpandableWorldObjects);
				GenWorld.cachedFrame = Time.frameCount;
				result = GenWorld.cachedTile;
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
					if (GenWorld.tmpWorldObjectsUnderMouse.Any())
					{
						int tile = GenWorld.tmpWorldObjectsUnderMouse[0].Tile;
						GenWorld.tmpWorldObjectsUnderMouse.Clear();
						result = tile;
						goto IL_00b8;
					}
				}
				Ray ray = worldCamera.ScreenPointToRay(clickPos * Prefs.UIScale);
				int worldLayerMask = WorldCameraManager.WorldLayerMask;
				RaycastHit hit = default(RaycastHit);
				result = ((!Physics.Raycast(ray, out hit, 1500f, worldLayerMask)) ? (-1) : Find.World.renderer.GetTileIDFromRayHit(hit));
			}
			goto IL_00b8;
			IL_00b8:
			return result;
		}
	}
}
