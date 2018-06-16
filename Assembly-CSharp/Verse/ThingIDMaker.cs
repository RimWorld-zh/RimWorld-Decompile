using System;

namespace Verse
{
	// Token: 0x02000CAF RID: 3247
	public static class ThingIDMaker
	{
		// Token: 0x0600477F RID: 18303 RVA: 0x0025A73C File Offset: 0x00258B3C
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
