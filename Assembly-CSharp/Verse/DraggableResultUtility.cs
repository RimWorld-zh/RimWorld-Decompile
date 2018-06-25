using System;

namespace Verse
{
	// Token: 0x02000EB5 RID: 3765
	internal static class DraggableResultUtility
	{
		// Token: 0x0600591D RID: 22813 RVA: 0x002DBB60 File Offset: 0x002D9F60
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
