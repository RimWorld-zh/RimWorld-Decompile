using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class WorldObjectSelectionUtility
	{
		public static IEnumerable<WorldObject> MultiSelectableWorldObjectsInScreenRectDistinct(Rect rect)
		{
			List<WorldObject> allObjects = Find.WorldObjects.AllWorldObjects;
			int i = 0;
			while (true)
			{
				if (i < allObjects.Count)
				{
					if (!allObjects[i].NeverMultiSelect && !allObjects[i].HiddenBehindTerrainNow())
					{
						if (ExpandableWorldObjectsUtility.IsExpanded(allObjects[i]))
						{
							if (rect.Overlaps(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(allObjects[i])))
							{
								yield return allObjects[i];
								/*Error: Unable to find new state assignment for yield return*/;
							}
						}
						else if (rect.Contains(allObjects[i].ScreenPos()))
							break;
					}
					i++;
					continue;
				}
				yield break;
			}
			yield return allObjects[i];
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos);
		}

		public static Vector2 ScreenPos(this WorldObject o)
		{
			Vector3 drawPos = o.DrawPos;
			return GenWorldUI.WorldToUIPosition(drawPos);
		}

		public static bool VisibleToCameraNow(this WorldObject o)
		{
			bool result;
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				result = false;
			}
			else if (o.HiddenBehindTerrainNow())
			{
				result = false;
			}
			else
			{
				Vector2 point = o.ScreenPos();
				result = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(point);
			}
			return result;
		}

		public static float DistanceToMouse(this WorldObject o, Vector2 mousePos)
		{
			Ray ray = Find.WorldCamera.ScreenPointToRay(mousePos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit raycastHit = default(RaycastHit);
			return (!Physics.Raycast(ray, out raycastHit, 1500f, worldLayerMask)) ? Vector3.Cross(ray.direction, o.DrawPos - ray.origin).magnitude : Vector3.Distance(raycastHit.point, o.DrawPos);
		}
	}
}
