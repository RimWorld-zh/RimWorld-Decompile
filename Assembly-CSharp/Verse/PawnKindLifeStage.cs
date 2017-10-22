namespace Verse
{
	public class PawnKindLifeStage
	{
		public string label = (string)null;

		public string labelPlural = (string)null;

		public string labelMale = (string)null;

		public string labelMalePlural = (string)null;

		public string labelFemale = (string)null;

		public string labelFemalePlural = (string)null;

		public GraphicData bodyGraphicData = null;

		public GraphicData femaleGraphicData = null;

		public GraphicData dessicatedBodyGraphicData = null;

		public BodyPartToDrop butcherBodyPart = null;

		public void ResolveReferences()
		{
			if (this.bodyGraphicData != null && this.bodyGraphicData.graphicClass == null)
			{
				this.bodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleGraphicData != null && this.femaleGraphicData.graphicClass == null)
			{
				this.femaleGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.dessicatedBodyGraphicData != null && this.dessicatedBodyGraphicData.graphicClass == null)
			{
				this.dessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
		}
	}
}
