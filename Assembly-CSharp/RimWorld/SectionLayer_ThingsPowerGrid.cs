using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038D RID: 909
	public class SectionLayer_ThingsPowerGrid : SectionLayer_Things
	{
		// Token: 0x06000FDD RID: 4061 RVA: 0x000853B4 File Offset: 0x000837B4
		public SectionLayer_ThingsPowerGrid(Section section) : base(section)
		{
			this.requireAddToMapMesh = false;
			this.relevantChangeTypes = MapMeshFlag.PowerGrid;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x000853D0 File Offset: 0x000837D0
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawPowerGrid)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x000853E4 File Offset: 0x000837E4
		protected override void TakePrintFrom(Thing t)
		{
			if (t.Faction == null || t.Faction == Faction.OfPlayer)
			{
				Building building = t as Building;
				if (building != null)
				{
					building.PrintForPowerGrid(this);
				}
			}
		}
	}
}
