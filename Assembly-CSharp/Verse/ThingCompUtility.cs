using System;

namespace Verse
{
	// Token: 0x02000E0B RID: 3595
	public static class ThingCompUtility
	{
		// Token: 0x0600516E RID: 20846 RVA: 0x0029ACA8 File Offset: 0x002990A8
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
