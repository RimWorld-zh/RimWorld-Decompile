using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E99 RID: 3737
	public static class DragSliderManager
	{
		// Token: 0x04003A4D RID: 14925
		private static bool dragging = false;

		// Token: 0x04003A4E RID: 14926
		private static float rootX;

		// Token: 0x04003A4F RID: 14927
		private static float lastRateFactor = 1f;

		// Token: 0x04003A50 RID: 14928
		private static DragSliderCallback draggingUpdateMethod;

		// Token: 0x04003A51 RID: 14929
		private static DragSliderCallback completedMethod;

		// Token: 0x0600582C RID: 22572 RVA: 0x002D364E File Offset: 0x002D1A4E
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x0600582D RID: 22573 RVA: 0x002D3658 File Offset: 0x002D1A58
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

		// Token: 0x0600582E RID: 22574 RVA: 0x002D36BC File Offset: 0x002D1ABC
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x0600582F RID: 22575 RVA: 0x002D36F0 File Offset: 0x002D1AF0
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x002D3718 File Offset: 0x002D1B18
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

		// Token: 0x06005831 RID: 22577 RVA: 0x002D3777 File Offset: 0x002D1B77
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
	}
}
