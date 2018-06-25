using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038F RID: 911
	public class SectionLayer_ThingsPowerGrid : SectionLayer_Things
	{
		// Token: 0x06000FE1 RID: 4065 RVA: 0x00085504 File Offset: 0x00083904
		public SectionLayer_ThingsPowerGrid(Section section) : base(section)
		{
			this.requireAddToMapMesh = false;
			this.relevantChangeTypes = MapMeshFlag.PowerGrid;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00085520 File Offset: 0x00083920
		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawPowerGrid)
			{
				base.DrawLayer();
			}
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00085534 File Offset: 0x00083934
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
