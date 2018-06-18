using System;

namespace Verse
{
	// Token: 0x02000CAE RID: 3246
	public static class ThingIDMaker
	{
		// Token: 0x0600477D RID: 18301 RVA: 0x0025A714 File Offset: 0x00258B14
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
