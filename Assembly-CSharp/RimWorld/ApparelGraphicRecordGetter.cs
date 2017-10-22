using Verse;

namespace RimWorld
{
	public static class ApparelGraphicRecordGetter
	{
		public static bool TryGetGraphicApparel(Apparel apparel, BodyType bodyType, out ApparelGraphicRecord rec)
		{
			if (bodyType == BodyType.Undefined)
			{
				Log.Error("Getting apparel graphic with undefined body type.");
				bodyType = BodyType.Male;
			}
			bool result;
			if (apparel.def.apparel.wornGraphicPath.NullOrEmpty())
			{
				rec = new ApparelGraphicRecord(null, null);
				result = false;
			}
			else
			{
				string path = (apparel.def.apparel.LastLayer != ApparelLayer.Overhead) ? (apparel.def.apparel.wornGraphicPath + "_" + bodyType.ToString()) : apparel.def.apparel.wornGraphicPath;
				Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(path, ShaderDatabase.Cutout, apparel.def.graphicData.drawSize, apparel.DrawColor);
				rec = new ApparelGraphicRecord(graphic, apparel);
				result = true;
			}
			return result;
		}
	}
}
