using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class MiscDebugDrawer
	{
		public static void DebugDrawInteractionCells()
		{
			Map visibleMap = Find.VisibleMap;
			if (visibleMap != null && DebugViewSettings.drawInteractionCells)
			{
				List<object>.Enumerator enumerator = Find.Selector.SelectedObjects.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						Thing thing = current as Thing;
						if (thing != null)
						{
							CellRenderer.RenderCell(thing.InteractionCell, 0.5f);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
		}
	}
}
