using UnityEngine;

namespace Verse
{
	public static class DragSliderManager
	{
		private static bool dragging = false;

		private static float rootX;

		private static float lastRateFactor = 1f;

		private static DragSliderCallback draggingUpdateMethod;

		private static DragSliderCallback completedMethod;

		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

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

		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			Vector2 mousePositionOnUI = UI.MousePositionOnUI;
			DragSliderManager.rootX = mousePositionOnUI.x;
		}

		private static float CurMouseOffset()
		{
			Vector2 mousePositionOnUI = UI.MousePositionOnUI;
			return mousePositionOnUI.x - DragSliderManager.rootX;
		}

		public static void DragSlidersOnGUI()
		{
			if (DragSliderManager.dragging && Event.current.type == EventType.MouseUp && Event.current.button == 0)
			{
				DragSliderManager.dragging = false;
				if ((object)DragSliderManager.completedMethod != null)
				{
					DragSliderManager.completedMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
				}
			}
		}

		public static void DragSlidersUpdate()
		{
			if (DragSliderManager.dragging && (object)DragSliderManager.draggingUpdateMethod != null)
			{
				DragSliderManager.draggingUpdateMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
			}
		}
	}
}
