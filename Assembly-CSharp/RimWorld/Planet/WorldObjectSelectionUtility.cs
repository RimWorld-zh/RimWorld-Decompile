using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008EE RID: 2286
	public static class WorldObjectSelectionUtility
	{
		// Token: 0x060034AD RID: 13485 RVA: 0x001C220C File Offset: 0x001C060C
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

		// Token: 0x060034AE RID: 13486 RVA: 0x001C2238 File Offset: 0x001C0638
		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x001C2258 File Offset: 0x001C0658
		public static Vector2 ScreenPos(this WorldObject o)
		{
			Vector3 drawPos = o.DrawPos;
			return GenWorldUI.WorldToUIPosition(drawPos);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x001C227C File Offset: 0x001C067C
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

		// Token: 0x060034B1 RID: 13489 RVA: 0x001C22E0 File Offset: 0x001C06E0
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
