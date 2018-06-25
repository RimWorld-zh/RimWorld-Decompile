using System;

namespace Verse
{
	// Token: 0x02000EB4 RID: 3764
	internal static class DraggableResultUtility
	{
		// Token: 0x0600591D RID: 22813 RVA: 0x002DB974 File Offset: 0x002D9D74
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
