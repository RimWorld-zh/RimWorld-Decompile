using System;

namespace Verse
{
	public static class ThingCompUtility
	{
		public static T TryGetComp<T>(this Thing thing) where T : ThingComp
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			T result;
			if (thingWithComps == null)
			{
				result = (T)((object)null);
			}
			else
			{
				result = thingWithComps.GetComp<T>();
			}
			return result;
		}
	}
}
