using System;

namespace Verse
{
	// Token: 0x02000E0C RID: 3596
	public static class ThingCompUtility
	{
		// Token: 0x06005170 RID: 20848 RVA: 0x0029ACC8 File Offset: 0x002990C8
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
