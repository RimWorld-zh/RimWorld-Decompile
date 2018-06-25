using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEB RID: 3307
	public static class ApparelGraphicRecordGetter
	{
		// Token: 0x060048D9 RID: 18649 RVA: 0x00263B40 File Offset: 0x00261F40
		public static bool TryGetGraphicApparel(Apparel apparel, BodyTypeDef bodyType, out ApparelGraphicRecord rec)
		{
			if (bodyType == null)
			{
				Log.Error("Getting apparel graphic with undefined body type.", false);
				bodyType = BodyTypeDefOf.Male;
			}
			bool result;
			if (apparel.def.apparel.wornGraphicPath.NullOrEmpty())
			{
				rec = new ApparelGraphicRecord(null, null);
				result = false;
			}
			else
			{
				string path;
				if (apparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead)
				{
					path = apparel.def.apparel.wornGraphicPath;
				}
				else
				{
					path = apparel.def.apparel.wornGraphicPath + "_" + bodyType.defName;
				}
				Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(path, ShaderDatabase.Cutout, apparel.def.graphicData.drawSize, apparel.DrawColor);
				rec = new ApparelGraphicRecord(graphic, apparel);
				result = true;
			}
			return result;
		}
	}
}
