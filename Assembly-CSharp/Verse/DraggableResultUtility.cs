using System;

namespace Verse
{
	// Token: 0x02000EB4 RID: 3764
	internal static class DraggableResultUtility
	{
		// Token: 0x060058FA RID: 22778 RVA: 0x002D9BC8 File Offset: 0x002D7FC8
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
