using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000680 RID: 1664
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x060022FB RID: 8955 RVA: 0x0012D55F File Offset: 0x0012B95F
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x0012D568 File Offset: 0x0012B968
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x0012D574 File Offset: 0x0012B974
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0012D5C0 File Offset: 0x0012B9C0
		public override void Print(SectionLayer layer, Thing parent)
		{
			CellRect.CellRectIterator iterator = parent.OccupiedRect().GetIterator();
			while (!iterator.Done())
			{
				IntVec3 cell = iterator.Current;
				Vector3 center = cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay);
				Printer_Plane.PrintPlane(layer, center, new Vector2(1f, 1f), base.LinkedDrawMatFrom(parent, cell), 0f, false, null, null, 0.01f, 0f);
				iterator.MoveNext();
			}
		}
	}
}
