using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008F0 RID: 2288
	public static class WorldObjectSelectionUtility
	{
		// Token: 0x060034B0 RID: 13488 RVA: 0x001C1EE4 File Offset: 0x001C02E4
		public static IEnumerable<WorldObject> MultiSelectableWorldObjectsInScreenRectDistinct(Rect rect)
		{
			List<WorldObject> allObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allObjects.Count; i++)
			{
				if (!allObjects[i].NeverMultiSelect)
				{
					if (!allObjects[i].HiddenBehindTerrainNow())
					{
						if (ExpandableWorldObjectsUtility.IsExpanded(allObjects[i]))
						{
							if (rect.Overlaps(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(allObjects[i])))
							{
								yield return allObjects[i];
							}
						}
						else if (rect.Contains(allObjects[i].ScreenPos()))
						{
							yield return allObjects[i];
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x001C1F10 File Offset: 0x001C0310
		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos);
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x001C1F30 File Offset: 0x001C0330
		public static Vector2 ScreenPos(this WorldObject o)
		{
			Vector3 drawPos = o.DrawPos;
			return GenWorldUI.WorldToUIPosition(drawPos);
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x001C1F54 File Offset: 0x001C0354
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
				Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
				result = rect.Contains(point);
			}
			return result;
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x001C1FB8 File Offset: 0x001C03B8
		public static float DistanceToMouse(this WorldObject o, Vector2 mousePos)
		{
			Ray ray = Find.WorldCamera.ScreenPointToRay(mousePos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit raycastHit;
			float result;
			if (Physics.Raycast(ray, out raycastHit, 1500f, worldLayerMask))
			{
				result = Vector3.Distance(raycastHit.point, o.DrawPos);
			}
			else
			{
				result = Vector3.Cross(ray.direction, o.DrawPos - ray.origin).magnitude;
			}
			return result;
		}
	}
}
