using System;

namespace Verse
{
	// Token: 0x02000E0B RID: 3595
	public static class ThingCompUtility
	{
		// Token: 0x06005186 RID: 20870 RVA: 0x0029C690 File Offset: 0x0029AA90
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
