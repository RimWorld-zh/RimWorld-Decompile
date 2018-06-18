using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E98 RID: 3736
	public static class DragSliderManager
	{
		// Token: 0x06005808 RID: 22536 RVA: 0x002D1912 File Offset: 0x002CFD12
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x002D191C File Offset: 0x002CFD1C
		public static bool DragSlider(Rect rect, float rateFactor, DragSliderCallback newStartMethod, DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			bool result;
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragSliderManager.lastRateFactor = rateFactor;
				newStartMethod(0f, rateFactor);
				DragSliderManager.StartDragSliding(newDraggingUpdateMethod, newCompletedMethod);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600580A RID: 22538 RVA: 0x002D1980 File Offset: 0x002CFD80
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x0600580B RID: 22539 RVA: 0x002D19B4 File Offset: 0x002CFDB4
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x002D19DC File Offset: 0x002CFDDC
		public static void DragSlidersOnGUI()
		{
			if (DragSliderManager.dragging)
			{
				if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
				{
					DragSliderManager.dragging = false;
					if (DragSliderManager.completedMethod != null)
					{
						DragSliderManager.completedMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
					}
				}
			}
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x002D1A3B File Offset: 0x002CFE3B
		public static void DragSlidersUpdate()
		{
			if (DragSliderManager.dragging)
			{
				if (DragSliderManager.draggingUpdateMethod != null)
				{
					DragSliderManager.draggingUpdateMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
				}
			}
		}

		// Token: 0x04003A3D RID: 14909
		private static bool dragging = false;

		// Token: 0x04003A3E RID: 14910
		private static float rootX;

		// Token: 0x04003A3F RID: 14911
		private static float lastRateFactor = 1f;

		// Token: 0x04003A40 RID: 14912
		private static DragSliderCallback draggingUpdateMethod;

		// Token: 0x04003A41 RID: 14913
		private static DragSliderCallback completedMethod;
	}
}
