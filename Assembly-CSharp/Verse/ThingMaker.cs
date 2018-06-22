using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000DF4 RID: 3572
	public static class ThingMaker
	{
		// Token: 0x06005076 RID: 20598 RVA: 0x00298124 File Offset: 0x00296524
		public static Thing MakeThing(ThingDef def, ThingDef stuff = null)
		{
			if (stuff != null && !stuff.IsStuff)
			{
				Log.Error(string.Concat(new object[]
				{
					"MakeThing error: Tried to make ",
					def,
					" from ",
					stuff,
					" which is not a stuff. Assigning default."
				}), false);
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (def.MadeFromStuff && stuff == null)
			{
				Log.Error("MakeThing error: " + def + " is madeFromStuff but stuff=null. Assigning default.", false);
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (!def.MadeFromStuff && stuff != null)
			{
				Log.Error(string.Concat(new object[]
				{
					"MakeThing error: ",
					def,
					" is not madeFromStuff but stuff=",
					stuff,
					". Setting to null."
				}), false);
				stuff = null;
			}
			Thing thing = (Thing)Activator.CreateInstance(def.thingClass);
			thing.def = def;
			thing.SetStuffDirect(stuff);
			thing.PostMake();
			return thing;
		}
	}
}
