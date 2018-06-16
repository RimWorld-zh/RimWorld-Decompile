using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD4 RID: 3540
	public static class GraphicGetter_NakedHumanlike
	{
		// Token: 0x06004F30 RID: 20272 RVA: 0x00293820 File Offset: 0x00291C20
		public static Graphic GetNakedBodyGraphic(BodyTypeDef bodyType, Shader shader, Color skinColor)
		{
			if (bodyType == null)
			{
				Log.Error("Getting naked body graphic with undefined body type.", false);
				bodyType = BodyTypeDefOf.Male;
			}
			string str = "Naked_" + bodyType.defName;
			string path = "Things/Pawn/Humanlike/Bodies/" + str;
			return GraphicDatabase.Get<Graphic_Multi>(path, shader, Vector2.one, skinColor);
		}

		// Token: 0x040034A6 RID: 13478
		private const string NakedBodyTextureFolderPath = "Things/Pawn/Humanlike/Bodies/";
	}
}
