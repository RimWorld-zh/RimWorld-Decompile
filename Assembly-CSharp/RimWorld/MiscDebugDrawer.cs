using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000988 RID: 2440
	public static class MiscDebugDrawer
	{
		// Token: 0x060036F0 RID: 14064 RVA: 0x001D6234 File Offset: 0x001D4634
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
