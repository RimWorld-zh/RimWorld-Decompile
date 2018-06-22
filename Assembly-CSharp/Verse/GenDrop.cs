using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F2C RID: 3884
	public static class GenDrop
	{
		// Token: 0x06005D5C RID: 23900 RVA: 0x002F589C File Offset: 0x002F3C9C
		public static bool TryDropSpawn(Thing thing, IntVec3 dropCell, Map map, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			bool result;
			if (map == null)
			{
				Log.Error("Dropped " + thing + " in a null map.", false);
				resultingThing = null;
				result = false;
			}
			else if (!dropCell.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Dropped ",
					thing,
					" out of bounds at ",
					dropCell
				}), false);
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
				result = GenPlace.TryPlaceThing(thing, dropCell, map, mode, out resultingThing, placedAction, nearPlaceValidator);
			}
			return result;
		}
	}
}
