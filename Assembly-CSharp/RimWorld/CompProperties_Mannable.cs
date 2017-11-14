using Verse;

namespace RimWorld
{
	public class CompProperties_Mannable : CompProperties
	{
		public WorkTags manWorkType;

		public CompProperties_Mannable()
		{
			base.compClass = typeof(CompMannable);
		}
	}
}
