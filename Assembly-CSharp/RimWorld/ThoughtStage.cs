using Verse;

namespace RimWorld
{
	public class ThoughtStage
	{
		[MustTranslate]
		public string label = (string)null;

		[MustTranslate]
		public string labelSocial = (string)null;

		[MustTranslate]
		public string description = (string)null;

		public float baseMoodEffect = 0f;

		public float baseOpinionOffset = 0f;

		public bool visible = true;
	}
}
