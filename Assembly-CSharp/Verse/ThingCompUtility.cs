using System;

namespace Verse
{
	// Token: 0x02000E08 RID: 3592
	public static class ThingCompUtility
	{
		// Token: 0x06005182 RID: 20866 RVA: 0x0029C284 File Offset: 0x0029A684
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
