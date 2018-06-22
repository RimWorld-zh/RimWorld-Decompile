using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000986 RID: 2438
	public static class MiscDebugDrawer
	{
		// Token: 0x060036EC RID: 14060 RVA: 0x001D5E20 File Offset: 0x001D4220
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
