namespace RimWorld
{
	public class StorytellerCompProperties_CategoryIndividualMTBByBiome : StorytellerCompProperties
	{
		public IncidentCategory category;

		public bool applyCaravanStealthFactor = false;

		public StorytellerCompProperties_CategoryIndividualMTBByBiome()
		{
			base.compClass = typeof(StorytellerComp_CategoryIndividualMTBByBiome);
		}
	}
}
