using System;
using Verse;

namespace RimWorld
{
	public static class ApparelGraphicRecordGetter
	{
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
