using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EAB RID: 3755
	public static class UI
	{
		// Token: 0x04003AFC RID: 15100
		public static int screenWidth;

		// Token: 0x04003AFD RID: 15101
		public static int screenHeight;

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06005888 RID: 22664 RVA: 0x002D6A18 File Offset: 0x002D4E18
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06005889 RID: 22665 RVA: 0x002D6A44 File Offset: 0x002D4E44
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
		// (get) Token: 0x0600588A RID: 22666 RVA: 0x002D6A78 File Offset: 0x002D4E78
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

		// Token: 0x0600588B RID: 22667 RVA: 0x002D6AB4 File Offset: 0x002D4EB4
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

		// Token: 0x0600588C RID: 22668 RVA: 0x002D6B5F File Offset: 0x002D4F5F
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x002D6B73 File Offset: 0x002D4F73
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x002D6B7C File Offset: 0x002D4F7C
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x002D6BA1 File Offset: 0x002D4FA1
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x002D6BB8 File Offset: 0x002D4FB8
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x002D6C00 File Offset: 0x002D5000
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x002D6C24 File Offset: 0x002D5024
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x002D6C7C File Offset: 0x002D507C
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x06005894 RID: 22676 RVA: 0x002D6C9C File Offset: 0x002D509C
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}
	}
}
