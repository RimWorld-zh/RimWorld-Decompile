using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067E RID: 1662
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x060022F9 RID: 8953 RVA: 0x0012D86F File Offset: 0x0012BC6F
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x0012D878 File Offset: 0x0012BC78
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x0012D884 File Offset: 0x0012BC84
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x0012D8D0 File Offset: 0x0012BCD0
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
