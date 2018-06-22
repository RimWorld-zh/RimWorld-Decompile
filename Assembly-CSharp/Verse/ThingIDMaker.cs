using System;

namespace Verse
{
	// Token: 0x02000CAB RID: 3243
	public static class ThingIDMaker
	{
		// Token: 0x06004786 RID: 18310 RVA: 0x0025BB04 File Offset: 0x00259F04
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
