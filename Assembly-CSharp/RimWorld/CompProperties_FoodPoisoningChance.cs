using Verse;

namespace RimWorld
{
	public class CompProperties_FoodPoisoningChance : CompProperties
	{
		public float chance = 0.02f;

		public bool humanlikeOnly;

		public CompProperties_FoodPoisoningChance()
		{
			base.compClass = typeof(CompFoodPoisoningChance);
		}
	}
}
