using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD0 RID: 3536
	public static class GraphicGetter_NakedHumanlike
	{
		// Token: 0x06004F43 RID: 20291 RVA: 0x00294DDC File Offset: 0x002931DC
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

		// Token: 0x040034AF RID: 13487
		private const string NakedBodyTextureFolderPath = "Things/Pawn/Humanlike/Bodies/";
	}
}
