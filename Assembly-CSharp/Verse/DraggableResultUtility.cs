using System;

namespace Verse
{
	// Token: 0x02000EB3 RID: 3763
	internal static class DraggableResultUtility
	{
		// Token: 0x060058F8 RID: 22776 RVA: 0x002D9C00 File Offset: 0x002D8000
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
