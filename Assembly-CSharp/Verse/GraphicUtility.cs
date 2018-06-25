using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000DD4 RID: 3540
	public static class GraphicUtility
	{
		// Token: 0x06004F48 RID: 20296 RVA: 0x00295244 File Offset: 0x00293644
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

		// Token: 0x06004F49 RID: 20297 RVA: 0x00295274 File Offset: 0x00293674
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
