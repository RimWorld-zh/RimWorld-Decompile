using System;
using Verse;

namespace RimWorld
{
	public static class GenThing
	{
		public static bool TryDropAndSetForbidden(Thing th, IntVec3 pos, Map map, ThingPlaceMode mode, out Thing resultingThing, bool forbidden)
		{
			bool result;
			if (GenDrop.TryDropSpawn(th, pos, map, ThingPlaceMode.Near, out resultingThing, (Action<Thing, int>)null))
			{
				if (resultingThing != null)
				{
					resultingThing.SetForbidden(forbidden, false);
				}
				result = true;
			}
			else
			{
				resultingThing = null;
				result = false;
			}
			return result;
		}
	}
}
