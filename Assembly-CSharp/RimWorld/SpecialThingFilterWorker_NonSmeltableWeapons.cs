using System;
using Verse;

namespace RimWorld
{
	public class SpecialThingFilterWorker_NonSmeltableWeapons : SpecialThingFilterWorker
	{
		public SpecialThingFilterWorker_NonSmeltableWeapons()
		{
		}

		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.Smeltable;
		}

		public override bool CanEverMatch(ThingDef def)
		{
			bool result;
			if (!def.IsWeapon)
			{
				result = false;
			}
			else
			{
				if (!def.thingCategories.NullOrEmpty<ThingCategoryDef>())
				{
					for (int i = 0; i < def.thingCategories.Count; i++)
					{
						for (ThingCategoryDef thingCategoryDef = def.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
						{
							if (thingCategoryDef == ThingCategoryDefOf.Weapons)
							{
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.smeltable && !def.MadeFromStuff;
		}
	}
}
