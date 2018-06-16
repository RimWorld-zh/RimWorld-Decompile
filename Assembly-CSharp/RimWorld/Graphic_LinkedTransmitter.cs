using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067F RID: 1663
	public class Graphic_LinkedTransmitter : Graphic_Linked
	{
		// Token: 0x060022F8 RID: 8952 RVA: 0x0012D43E File Offset: 0x0012B83E
		public Graphic_LinkedTransmitter(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x0012D448 File Offset: 0x0012B848
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && (base.ShouldLinkWith(c, parent) || parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null);
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x0012D4A0 File Offset: 0x0012B8A0
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
