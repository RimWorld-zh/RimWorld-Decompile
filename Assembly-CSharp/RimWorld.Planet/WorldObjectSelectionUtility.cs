using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class WorldObjectSelectionUtility
	{
		[DebuggerHidden]
		public static IEnumerable<WorldObject> MultiSelectableWorldObjectsInScreenRectDistinct(Rect rect)
		{
			WorldObjectSelectionUtility.<MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator19E <MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator19E = new WorldObjectSelectionUtility.<MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator19E();
			<MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator19E.rect = rect;
			<MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator19E.<$>rect = rect;
			WorldObjectSelectionUtility.<MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator19E expr_15 = <MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator19E;
			expr_15.$PC = -2;
			return expr_15;
		}

		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			Vector3 normalized = o.DrawPos.normalized;
			Vector3 currentlyLookingAtPointOnSphere = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
			return Vector3.Angle(normalized, currentlyLookingAtPointOnSphere) > 73f;
		}

		public static Vector2 ScreenPos(this WorldObject o)
		{
			Vector3 drawPos = o.DrawPos;
			return GenWorldUI.WorldToUIPosition(drawPos);
		}

		public static bool VisibleToCameraNow(this WorldObject o)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return false;
			}
			if (o.HiddenBehindTerrainNow())
			{
				return false;
			}
			Vector2 point = o.ScreenPos();
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			return rect.Contains(point);
		}

		public static float DistanceToMouse(this WorldObject o, Vector2 mousePos)
		{
			Ray ray = Find.WorldCamera.ScreenPointToRay(mousePos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1500f, worldLayerMask))
			{
				return Vector3.Distance(raycastHit.point, o.DrawPos);
			}
			return Vector3.Cross(ray.direction, o.DrawPos - ray.origin).magnitude;
		}
	}
}
