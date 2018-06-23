using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E97 RID: 3735
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

		// Token: 0x06005828 RID: 22568 RVA: 0x002D3522 File Offset: 0x002D1922
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x06005829 RID: 22569 RVA: 0x002D352C File Offset: 0x002D192C
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

		// Token: 0x0600582A RID: 22570 RVA: 0x002D3590 File Offset: 0x002D1990
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x0600582B RID: 22571 RVA: 0x002D35C4 File Offset: 0x002D19C4
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x0600582C RID: 22572 RVA: 0x002D35EC File Offset: 0x002D19EC
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

		// Token: 0x0600582D RID: 22573 RVA: 0x002D364B File Offset: 0x002D1A4B
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
