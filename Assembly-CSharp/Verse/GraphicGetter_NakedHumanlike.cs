using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public static class GraphicGetter_NakedHumanlike
	{
		private const string NakedBodyTextureFolderPath = "Things/Pawn/Humanlike/Bodies/";

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
