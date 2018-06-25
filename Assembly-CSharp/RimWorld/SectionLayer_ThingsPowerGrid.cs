using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038F RID: 911
	public class SectionLayer_ThingsPowerGrid : SectionLayer_Things
	{
		// Token: 0x06000FE0 RID: 4064 RVA: 0x00085514 File Offset: 0x00083914
		public SectionLayer_ThingsPowerGrid(Section section) : base(section)
		{
			this.requireAddToMapMesh = false;
			this.relevantChangeTypes = MapMeshFlag.PowerGrid;
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00085530 File Offset: 0x00083930
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawPowerGrid)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00085544 File Offset: 0x00083944
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
