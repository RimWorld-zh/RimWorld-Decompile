using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD3 RID: 3539
	public static class GraphicGetter_NakedHumanlike
	{
		// Token: 0x040034B6 RID: 13494
		private const string NakedBodyTextureFolderPath = "Things/Pawn/Humanlike/Bodies/";

		// Token: 0x06004F47 RID: 20295 RVA: 0x002951E8 File Offset: 0x002935E8
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
