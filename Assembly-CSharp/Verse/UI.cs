using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EA9 RID: 3753
	public static class UI
	{
		// Token: 0x04003AFC RID: 15100
		public static int screenWidth;

		// Token: 0x04003AFD RID: 15101
		public static int screenHeight;

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06005884 RID: 22660 RVA: 0x002D68EC File Offset: 0x002D4CEC
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06005885 RID: 22661 RVA: 0x002D6918 File Offset: 0x002D4D18
		public static Vector2 MousePositionOnUIInverted
		{
			get
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
				return mousePositionOnUI;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06005886 RID: 22662 RVA: 0x002D694C File Offset: 0x002D4D4C
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

		// Token: 0x06005887 RID: 22663 RVA: 0x002D6988 File Offset: 0x002D4D88
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

		// Token: 0x06005888 RID: 22664 RVA: 0x002D6A33 File Offset: 0x002D4E33
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x06005889 RID: 22665 RVA: 0x002D6A47 File Offset: 0x002D4E47
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x0600588A RID: 22666 RVA: 0x002D6A50 File Offset: 0x002D4E50
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x0600588B RID: 22667 RVA: 0x002D6A75 File Offset: 0x002D4E75
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x002D6A8C File Offset: 0x002D4E8C
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x002D6AD4 File Offset: 0x002D4ED4
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x002D6AF8 File Offset: 0x002D4EF8
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x002D6B50 File Offset: 0x002D4F50
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x002D6B70 File Offset: 0x002D4F70
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}
	}
}
