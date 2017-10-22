using Verse;

namespace RimWorld
{
	public class SectionLayer_ThingsPowerGrid : SectionLayer_Things
	{
		public SectionLayer_ThingsPowerGrid(Section section) : base(section)
		{
			base.requireAddToMapMesh = false;
			base.relevantChangeTypes = MapMeshFlag.PowerGrid;
		}

		public override void DrawLayer()
		{
			if (OverlayDrawHandler.ShouldDrawPowerGrid)
			{
				base.DrawLayer();
			}
		}

		protected override void TakePrintFrom(Thing t)
		{
			Building building = t as Building;
			if (building != null)
			{
				building.PrintForPowerGrid(this);
			}
		}
	}
}
