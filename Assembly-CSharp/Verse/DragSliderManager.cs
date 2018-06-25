using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9A RID: 3738
	public static class DragSliderManager
	{
		// Token: 0x04003A55 RID: 14933
		private static bool dragging = false;

		// Token: 0x04003A56 RID: 14934
		private static float rootX;

		// Token: 0x04003A57 RID: 14935
		private static float lastRateFactor = 1f;

		// Token: 0x04003A58 RID: 14936
		private static DragSliderCallback draggingUpdateMethod;

		// Token: 0x04003A59 RID: 14937
		private static DragSliderCallback completedMethod;

		// Token: 0x0600582C RID: 22572 RVA: 0x002D383A File Offset: 0x002D1C3A
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x0600582D RID: 22573 RVA: 0x002D3844 File Offset: 0x002D1C44
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

		// Token: 0x0600582E RID: 22574 RVA: 0x002D38A8 File Offset: 0x002D1CA8
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x0600582F RID: 22575 RVA: 0x002D38DC File Offset: 0x002D1CDC
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x002D3904 File Offset: 0x002D1D04
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

		// Token: 0x06005831 RID: 22577 RVA: 0x002D3963 File Offset: 0x002D1D63
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
