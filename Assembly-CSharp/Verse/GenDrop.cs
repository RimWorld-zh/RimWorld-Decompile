using System;
using Verse.Sound;

namespace Verse
{
	public static class GenDrop
	{
		public static bool TryDropSpawn(Thing thing, IntVec3 dropCell, Map map, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			bool result;
			if (map == null)
			{
				Log.Error("Dropped " + thing + " in a null map.");
				resultingThing = null;
				result = false;
			}
			else if (!dropCell.InBounds(map))
			{
				Log.Error("Dropped " + thing + " out of bounds at " + dropCell);
				resultingThing = null;
				result = false;
			}
			else if (thing.def.destroyOnDrop)
			{
				thing.Destroy(DestroyMode.Vanish);
				resultingThing = null;
				result = true;
			}
			else
			{
				if (thing.def.soundDrop != null)
				{
					thing.def.soundDrop.PlayOneShot(new TargetInfo(dropCell, map, false));
				}
				result = GenPlace.TryPlaceThing(thing, dropCell, map, mode, out resultingThing, placedAction);
			}
			return result;
		}
	}
}
