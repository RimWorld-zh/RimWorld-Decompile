using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000680 RID: 1664
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x060022FD RID: 8957 RVA: 0x0012D5D7 File Offset: 0x0012B9D7
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0012D5E0 File Offset: 0x0012B9E0
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x0012D5EC File Offset: 0x0012B9EC
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x0012D638 File Offset: 0x0012BA38
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
