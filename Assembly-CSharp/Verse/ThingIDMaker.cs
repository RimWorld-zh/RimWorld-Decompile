using System;

namespace Verse
{
	// Token: 0x02000CAD RID: 3245
	public static class ThingIDMaker
	{
		// Token: 0x06004789 RID: 18313 RVA: 0x0025BBE0 File Offset: 0x00259FE0
		public static void GiveIDTo(Thing t)
		{
			if (t.def.HasThingIDNumber)
			{
				if (t.thingIDNumber != -1)
				{
					Log.Error(string.Concat(new object[]
					{
						"Giving ID to ",
						t,
						" which already has id ",
						t.thingIDNumber
					}), false);
				}
				t.thingIDNumber = Find.UniqueIDsManager.GetNextThingID();
			}
		}
	}
}
