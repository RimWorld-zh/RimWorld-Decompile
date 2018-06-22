using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067B RID: 1659
	public class Graphic_LinkedTransmitter : Graphic_Linked
	{
		// Token: 0x060022F2 RID: 8946 RVA: 0x0012D5FE File Offset: 0x0012B9FE
		public Graphic_LinkedTransmitter(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0012D608 File Offset: 0x0012BA08
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && (base.ShouldLinkWith(c, parent) || parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null);
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x0012D660 File Offset: 0x0012BA60
		public override void Print(SectionLayer layer, Thing thing)
		{
			base.Print(layer, thing);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = thing.Position + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(thing.Map))
				{
					Building transmitter = intVec.GetTransmitter(thing.Map);
					if (transmitter != null && !transmitter.def.graphicData.Linked)
					{
						Material mat = base.LinkedDrawMatFrom(thing, intVec);
						Printer_Plane.PrintPlane(layer, intVec.ToVector3ShiftedWithAltitude(thing.def.Altitude), Vector2.one, mat, 0f, false, null, null, 0.01f, 0f);
					}
				}
			}
		}
	}
}
