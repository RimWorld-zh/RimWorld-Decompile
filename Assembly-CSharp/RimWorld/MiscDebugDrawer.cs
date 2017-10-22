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
				foreach (object selectedObject in Find.Selector.SelectedObjects)
				{
					Thing thing = selectedObject as Thing;
					if (thing != null)
					{
						CellRenderer.RenderCell(thing.InteractionCell, 0.5f);
					}
				}
			}
		}
	}
}
