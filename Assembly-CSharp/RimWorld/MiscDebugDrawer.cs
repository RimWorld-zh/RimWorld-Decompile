using System;
using Verse;

namespace RimWorld
{
	public static class MiscDebugDrawer
	{
		public static void DebugDrawInteractionCells()
		{
			if (Find.CurrentMap != null)
			{
				if (DebugViewSettings.drawInteractionCells)
				{
					foreach (object obj in Find.Selector.SelectedObjects)
					{
						Thing thing = obj as Thing;
						if (thing != null)
						{
							CellRenderer.RenderCell(thing.InteractionCell, 0.5f);
						}
					}
				}
			}
		}
	}
}
