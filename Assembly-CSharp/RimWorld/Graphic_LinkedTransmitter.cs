using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Graphic_LinkedTransmitter : Graphic_Linked
	{
		public Graphic_LinkedTransmitter(Graphic subGraphic) : base(subGraphic)
		{
		}

		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return (byte)(c.InBounds(parent.Map) ? ((base.ShouldLinkWith(c, parent) || parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null) ? 1 : 0) : 0) != 0;
		}

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
						Printer_Plane.PrintPlane(layer, intVec.ToVector3ShiftedWithAltitude(thing.def.Altitude), Vector2.one, mat, 0f, false, null, null, 0.01f);
					}
				}
			}
		}
	}
}
