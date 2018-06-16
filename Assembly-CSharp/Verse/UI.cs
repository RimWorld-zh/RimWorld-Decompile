using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EAB RID: 3755
	public static class UI
	{
		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06005866 RID: 22630 RVA: 0x002D4CCC File Offset: 0x002D30CC
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06005867 RID: 22631 RVA: 0x002D4CF8 File Offset: 0x002D30F8
		public static Vector2 MousePositionOnUIInverted
		{
			get
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
				return mousePositionOnUI;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06005868 RID: 22632 RVA: 0x002D4D2C File Offset: 0x002D312C
		public static Vector2 MousePosUIInvertedUseEventIfCan
		{
			get
			{
				Vector2 result;
				if (Event.current != null)
				{
					result = UI.GUIToScreenPoint(Event.current.mousePosition);
				}
				else
				{
					result = UI.MousePositionOnUIInverted;
				}
				return result;
			}
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x002D4D68 File Offset: 0x002D3168
		public static void ApplyUIScale()
		{
			if (Prefs.UIScale == 1f || !LongEventHandler.CanApplyUIScaleNow)
			{
				UI.screenWidth = Screen.width;
				UI.screenHeight = Screen.height;
			}
			else
			{
				UI.screenWidth = Mathf.RoundToInt((float)Screen.width / Prefs.UIScale);
				UI.screenHeight = Mathf.RoundToInt((float)Screen.height / Prefs.UIScale);
				float uiscale = Prefs.UIScale;
				float uiscale2 = Prefs.UIScale;
				GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(uiscale, uiscale2, 1f));
			}
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x002D4E13 File Offset: 0x002D3213
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x002D4E27 File Offset: 0x002D3227
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x002D4E30 File Offset: 0x002D3230
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x0600586D RID: 22637 RVA: 0x002D4E55 File Offset: 0x002D3255
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x002D4E6C File Offset: 0x002D326C
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x002D4EB4 File Offset: 0x002D32B4
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x002D4ED8 File Offset: 0x002D32D8
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x002D4F30 File Offset: 0x002D3330
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x002D4F50 File Offset: 0x002D3350
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}

		// Token: 0x04003AED RID: 15085
		public static int screenWidth;

		// Token: 0x04003AEE RID: 15086
		public static int screenHeight;
	}
}
