using Verse;

namespace RimWorld
{
	public class CompProperties_ChangeableProjectile : CompProperties
	{
		public CompProperties_ChangeableProjectile()
		{
			base.compClass = typeof(CompChangeableProjectile);
		}
	}
}
