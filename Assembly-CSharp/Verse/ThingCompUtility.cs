using System;

namespace Verse
{
	// Token: 0x02000E0A RID: 3594
	public static class ThingCompUtility
	{
		// Token: 0x06005186 RID: 20870 RVA: 0x0029C3B0 File Offset: 0x0029A7B0
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
