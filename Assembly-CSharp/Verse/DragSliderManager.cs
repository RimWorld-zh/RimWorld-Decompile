using System;
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
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static DragSliderManager()
		{
		}
	}
}
