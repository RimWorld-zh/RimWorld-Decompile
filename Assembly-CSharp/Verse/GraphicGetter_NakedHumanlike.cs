using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD2 RID: 3538
	public static class GraphicGetter_NakedHumanlike
	{
		// Token: 0x040034AF RID: 13487
		private const string NakedBodyTextureFolderPath = "Things/Pawn/Humanlike/Bodies/";

		// Token: 0x06004F47 RID: 20295 RVA: 0x00294F08 File Offset: 0x00293308
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
	}
}
