using Verse;

namespace RimWorld
{
	public class CompProperties_Maintainable : CompProperties
	{
		public int ticksHealthy = 1000;

		public int ticksNeedsMaintenance = 1000;

		public int damagePerTickRare = 10;

		public CompProperties_Maintainable()
		{
			base.compClass = typeof(CompMaintainable);
		}
	}
}
