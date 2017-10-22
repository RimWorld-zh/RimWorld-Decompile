using Verse;

namespace RimWorld
{
	public class CompProperties_Art : CompProperties
	{
		public RulePackDef nameMaker;

		public RulePackDef descriptionMaker;

		public QualityCategory minQualityForArtistic = QualityCategory.Awful;

		public bool mustBeFullGrave = false;

		public bool canBeEnjoyedAsArt = false;

		public CompProperties_Art()
		{
			base.compClass = typeof(CompArt);
		}
	}
}
