using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000DD1 RID: 3537
	public static class GraphicUtility
	{
		// Token: 0x06004F44 RID: 20292 RVA: 0x00294E38 File Offset: 0x00293238
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

		// Token: 0x06004F45 RID: 20293 RVA: 0x00294E68 File Offset: 0x00293268
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
