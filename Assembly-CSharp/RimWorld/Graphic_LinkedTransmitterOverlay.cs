using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067E RID: 1662
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x060022F8 RID: 8952 RVA: 0x0012DAD7 File Offset: 0x0012BED7
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x0012DAE0 File Offset: 0x0012BEE0
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x0012DAEC File Offset: 0x0012BEEC
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x0012DB38 File Offset: 0x0012BF38
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
