using Verse;

namespace RimWorld
{
	public class CompProperties_Hatcher : CompProperties
	{
		public float hatcherDaystoHatch = 1f;

		public PawnKindDef hatcherPawn = null;

		public CompProperties_Hatcher()
		{
			base.compClass = typeof(CompHatcher);
		}
	}
}
