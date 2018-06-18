using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098A RID: 2442
	public static class MiscDebugDrawer
	{
		// Token: 0x060036F3 RID: 14067 RVA: 0x001D5C24 File Offset: 0x001D4024
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
