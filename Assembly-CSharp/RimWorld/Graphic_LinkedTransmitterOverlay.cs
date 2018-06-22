using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067C RID: 1660
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x060022F5 RID: 8949 RVA: 0x0012D71F File Offset: 0x0012BB1F
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x0012D728 File Offset: 0x0012BB28
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x0012D734 File Offset: 0x0012BB34
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x0012D780 File Offset: 0x0012BB80
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
