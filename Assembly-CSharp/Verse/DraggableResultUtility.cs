using System;

namespace Verse
{
	// Token: 0x02000EB2 RID: 3762
	internal static class DraggableResultUtility
	{
		// Token: 0x06005919 RID: 22809 RVA: 0x002DB848 File Offset: 0x002D9C48
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
