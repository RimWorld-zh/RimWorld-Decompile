using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200059F RID: 1439
	public static class ExpandableWorldObjectsUtility
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001B6B RID: 7019 RVA: 0x000EC834 File Offset: 0x000EAC34
		public static float TransitionPct
		{
			get
			{
				float result;
				if (!Find.PlaySettings.showExpandingIcons)
				{
					result = 0f;
				}
				else
				{
					result = ExpandableWorldObjectsUtility.transitionPct;
				}
				return result;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x000EC868 File Offset: 0x000EAC68
		public static float ExpandMoreTransitionPct
		{
			get
			{
				float result;
				if (!Find.PlaySettings.showExpandingIcons)
				{
					result = 0f;
				}
				else
				{
					result = ExpandableWorldObjectsUtility.expandMoreTransitionPct;
				}
				return result;
			}
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x000EC89C File Offset: 0x000EAC9C
		public static void ExpandableWorldObjectsUpdate()
		{
			float num = Time.deltaTime * 3f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose)
			{
				ExpandableWorldObjectsUtility.transitionPct -= num;
			}
			else
			{
				ExpandableWorldObjectsUtility.transitionPct += num;
			}
			ExpandableWorldObjectsUtility.transitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.transitionPct);
			float num2 = Time.deltaTime * 4f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.Far)
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct -= num2;
			}
			else
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct += num2;
			}
			ExpandableWorldObjectsUtility.expandMoreTransitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.expandMoreTransitionPct);
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000EC93C File Offset: 0x000EAD3C
		public static void ExpandableWorldObjectsOnGUI()
		{
			if (ExpandableWorldObjectsUtility.TransitionPct != 0f)
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
					if (worldObject.def.expandingIcon)
					{
						if (!worldObject.HiddenBehindTerrainNow())
						{
							Color expandingIconColor = worldObject.ExpandingIconColor;
							expandingIconColor.a = ExpandableWorldObjectsUtility.TransitionPct;
							if (worldTargeter.IsTargetedNow(worldObject, worldObjectsUnderMouse))
							{
								float num = GenMath.LerpDouble(-1f, 1f, 0.7f, 1f, Mathf.Sin(Time.time * 8f));
								expandingIconColor.r *= num;
								expandingIconColor.g *= num;
								expandingIconColor.b *= num;
							}
							GUI.color = expandingIconColor;
							GUI.DrawTexture(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject), worldObject.ExpandingIcon);
						}
					}
				}
				ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
				GUI.color = Color.white;
			}
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x000ECA9C File Offset: 0x000EAE9C
		public static Rect ExpandedIconScreenRect(WorldObject o)
		{
			Vector2 vector = o.ScreenPos();
			float num;
			if (o.ExpandMore)
			{
				num = Mathf.Lerp(30f, 40.5f, ExpandableWorldObjectsUtility.ExpandMoreTransitionPct);
			}
			else
			{
				num = 30f;
			}
			return new Rect(vector.x - num / 2f, vector.y - num / 2f, num, num);
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000ECB0C File Offset: 0x000EAF0C
		public static bool IsExpanded(WorldObject o)
		{
			return ExpandableWorldObjectsUtility.TransitionPct > 0.5f && o.def.expandingIcon;
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x000ECB40 File Offset: 0x000EAF40
		public static void GetExpandedWorldObjectUnderMouse(Vector2 mousePos, List<WorldObject> outList)
		{
			outList.Clear();
			Vector2 point = mousePos;
			point.y = (float)UI.screenHeight - point.y;
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				WorldObject worldObject = allWorldObjects[i];
				if (ExpandableWorldObjectsUtility.IsExpanded(worldObject))
				{
					if (ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject).Contains(point))
					{
						if (!worldObject.HiddenBehindTerrainNow())
						{
							outList.Add(worldObject);
						}
					}
				}
			}
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(outList);
			outList.Reverse();
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x000ECBE4 File Offset: 0x000EAFE4
		private static void SortByExpandingIconPriority(List<WorldObject> worldObjects)
		{
			worldObjects.SortBy(delegate(WorldObject x)
			{
				float num = x.ExpandingIconPriority;
				if (x.Faction != null && x.Faction.IsPlayer)
				{
					num += 0.001f;
				}
				return num;
			}, (WorldObject x) => x.ID);
		}

		// Token: 0x04001027 RID: 4135
		private static float transitionPct;

		// Token: 0x04001028 RID: 4136
		private static float expandMoreTransitionPct;

		// Token: 0x04001029 RID: 4137
		private static List<WorldObject> tmpWorldObjects = new List<WorldObject>();

		// Token: 0x0400102A RID: 4138
		private const float WorldObjectIconSize = 30f;

		// Token: 0x0400102B RID: 4139
		private const float ExpandMoreWorldObjectIconSizeFactor = 1.35f;

		// Token: 0x0400102C RID: 4140
		private const float TransitionSpeed = 3f;

		// Token: 0x0400102D RID: 4141
		private const float ExpandMoreTransitionSpeed = 4f;
	}
}
