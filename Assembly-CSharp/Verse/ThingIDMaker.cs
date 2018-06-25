using System;

namespace Verse
{
	// Token: 0x02000CAE RID: 3246
	public static class ThingIDMaker
	{
		// Token: 0x06004789 RID: 18313 RVA: 0x0025BEC0 File Offset: 0x0025A2C0
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
