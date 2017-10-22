namespace Verse
{
	public class CompProperties_Lifespan : CompProperties
	{
		public int lifespanTicks = 100;

		public CompProperties_Lifespan()
		{
			base.compClass = typeof(CompLifespan);
		}
	}
}
