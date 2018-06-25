using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EAC RID: 3756
	public static class UI
	{
		// Token: 0x04003B04 RID: 15108
		public static int screenWidth;

		// Token: 0x04003B05 RID: 15109
		public static int screenHeight;

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06005888 RID: 22664 RVA: 0x002D6C04 File Offset: 0x002D5004
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06005889 RID: 22665 RVA: 0x002D6C30 File Offset: 0x002D5030
		public static Vector2 MousePositionOnUIInverted
		{
			get
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
				return mousePositionOnUI;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x0600588A RID: 22666 RVA: 0x002D6C64 File Offset: 0x002D5064
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

		// Token: 0x0600588B RID: 22667 RVA: 0x002D6CA0 File Offset: 0x002D50A0
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

		// Token: 0x0600588C RID: 22668 RVA: 0x002D6D4B File Offset: 0x002D514B
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x002D6D5F File Offset: 0x002D515F
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x002D6D68 File Offset: 0x002D5168
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x002D6D8D File Offset: 0x002D518D
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x002D6DA4 File Offset: 0x002D51A4
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x002D6DEC File Offset: 0x002D51EC
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x002D6E10 File Offset: 0x002D5210
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x002D6E68 File Offset: 0x002D5268
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x06005894 RID: 22676 RVA: 0x002D6E88 File Offset: 0x002D5288
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}
	}
}
