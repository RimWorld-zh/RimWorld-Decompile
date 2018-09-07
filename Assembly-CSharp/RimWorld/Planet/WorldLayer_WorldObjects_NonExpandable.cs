using System;

namespace RimWorld.Planet
{
	public class WorldLayer_WorldObjects_NonExpandable : WorldLayer_WorldObjects
	{
		public WorldLayer_WorldObjects_NonExpandable()
		{
		}

		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
