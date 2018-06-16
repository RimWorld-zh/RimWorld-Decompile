using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000DD5 RID: 3541
	public static class GraphicUtility
	{
		// Token: 0x06004F31 RID: 20273 RVA: 0x0029387C File Offset: 0x00291C7C
		public static Graphic ExtractInnerGraphicFor(this Graphic outerGraphic, Thing thing)
		{
			Graphic_Random graphic_Random = outerGraphic as Graphic_Random;
			Graphic result;
			if (graphic_Random != null)
			{
				result = graphic_Random.SubGraphicFor(thing);
			}
			else
			{
				result = outerGraphic;
			}
			return result;
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x002938AC File Offset: 0x00291CAC
		public static Graphic_Linked WrapLinked(Graphic subGraphic, LinkDrawerType linkDrawerType)
		{
			Graphic_Linked result;
			switch (linkDrawerType)
			{
			case LinkDrawerType.None:
				result = null;
				break;
			case LinkDrawerType.Basic:
				result = new Graphic_Linked(subGraphic);
				break;
			case LinkDrawerType.CornerFiller:
				result = new Graphic_LinkedCornerFiller(subGraphic);
				break;
			case LinkDrawerType.Transmitter:
				result = new Graphic_LinkedTransmitter(subGraphic);
				break;
			case LinkDrawerType.TransmitterOverlay:
				result = new Graphic_LinkedTransmitterOverlay(subGraphic);
				break;
			default:
				throw new ArgumentException();
			}
			return result;
		}
	}
}
