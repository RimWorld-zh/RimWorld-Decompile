using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class ExpandableWorldObjectsUtility
	{
		private static float transitionPct;

		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();

		private const float WorldObjectSize = 30f;

		public static float TransitionPct
		{
			get
			{
				return (float)(Find.PlaySettings.showExpandingIcons ? ExpandableWorldObjectsUtility.transitionPct : 0.0);
			}
		}

		public static void ExpandableWorldObjectsUpdate()
		{
			float num = (float)(Time.deltaTime * 3.0);
			if ((int)Find.WorldCameraDriver.CurrentZoom <= 0)
			{
				ExpandableWorldObjectsUtility.transitionPct -= num;
			}
			else
			{
				ExpandableWorldObjectsUtility.transitionPct += num;
			}
			ExpandableWorldObjectsUtility.transitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.transitionPct);
		}

		public static void ExpandableWorldObjectsOnGUI()
		{
			if (ExpandableWorldObjectsUtility.TransitionPct != 0.0)
			{
				ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
				ExpandableWorldObjectsUtility.tmpWorldObjects.AddRange(Find.WorldObjects.AllWorldObjects);
				ExpandableWorldObjectsUtility.SortByExpandingIconPriority(ExpandableWorldObjectsUtility.tmpWorldObjects);
				WorldTargeter worldTargeter = Find.WorldTargeter;
				List<WorldObject> worldObjectsUnderMouse = null;
				if (worldTargeter.IsTargeting)
				{
					worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
				}
				for (int i = 0; i < ExpandableWorldObjectsUtility.tmpWorldObjects.Count; i++)
				{
					WorldObject worldObject = ExpandableWorldObjectsUtility.tmpWorldObjects[i];
					if (worldObject.def.expandingIcon && !worldObject.HiddenBehindTerrainNow())
					{
						Color expandingIconColor = worldObject.ExpandingIconColor;
						expandingIconColor.a = ExpandableWorldObjectsUtility.TransitionPct;
						if (worldTargeter.IsTargetedNow(worldObject, worldObjectsUnderMouse))
						{
							float num = GenMath.LerpDouble(-1f, 1f, 0.7f, 1f, Mathf.Sin((float)(Time.time * 8.0)));
							expandingIconColor.r *= num;
							expandingIconColor.g *= num;
							expandingIconColor.b *= num;
						}
						GUI.color = expandingIconColor;
						GUI.DrawTexture(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject), worldObject.ExpandingIcon);
					}
				}
				ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
				GUI.color = Color.white;
			}
		}

		public static Rect ExpandedIconScreenRect(WorldObject o)
		{
			Vector2 vector = o.ScreenPos();
			return new Rect((float)(vector.x - 15.0), (float)(vector.y - 15.0), 30f, 30f);
		}

		public static bool IsExpanded(WorldObject o)
		{
			return ExpandableWorldObjectsUtility.TransitionPct > 0.5 && o.def.expandingIcon;
		}

		public static void GetExpandedWorldObjectUnderMouse(Vector2 mousePos, List<WorldObject> outList)
		{
			outList.Clear();
			Vector2 point = mousePos;
			point.y = (float)UI.screenHeight - point.y;
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				WorldObject worldObject = allWorldObjects[i];
				if (ExpandableWorldObjectsUtility.IsExpanded(worldObject) && ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject).Contains(point) && !worldObject.HiddenBehindTerrainNow())
				{
					outList.Add(worldObject);
				}
			}
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(outList);
			outList.Reverse();
		}

		private static void SortByExpandingIconPriority(List<WorldObject> worldObjects)
		{
			worldObjects.SortBy((Func<WorldObject, float>)delegate(WorldObject x)
			{
				float num = x.ExpandingIconPriority;
				if (x.Faction != null && x.Faction.IsPlayer)
				{
					num = (float)(num + 0.0010000000474974513);
				}
				return num;
			}, (Func<WorldObject, int>)((WorldObject x) => x.ID));
		}
	}
}
