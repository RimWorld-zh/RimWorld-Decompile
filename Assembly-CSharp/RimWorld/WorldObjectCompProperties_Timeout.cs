using RimWorld.Planet;

namespace RimWorld
{
	public class WorldObjectCompProperties_Timeout : WorldObjectCompProperties
	{
		public WorldObjectCompProperties_Timeout()
		{
			base.compClass = typeof(TimeoutComp);
		}
	}
}
