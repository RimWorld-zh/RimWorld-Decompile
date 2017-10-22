namespace Verse
{
	public static class ThingCompUtility
	{
		public static T TryGetComp<T>(this Thing thing) where T : ThingComp
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			return (thingWithComps != null) ? thingWithComps.GetComp<T>() : ((T)null);
		}
	}
}
