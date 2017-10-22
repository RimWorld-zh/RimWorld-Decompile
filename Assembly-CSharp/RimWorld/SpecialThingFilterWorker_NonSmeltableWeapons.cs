using Verse;

namespace RimWorld
{
	public class SpecialThingFilterWorker_NonSmeltableWeapons : SpecialThingFilterWorker
	{
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
				if (!def.thingCategories.NullOrEmpty())
				{
					for (int i = 0; i < def.thingCategories.Count; i++)
					{
						for (ThingCategoryDef thingCategoryDef = def.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
						{
							if (thingCategoryDef == ThingCategoryDefOf.Weapons)
								goto IL_004a;
						}
					}
				}
				result = false;
			}
			goto IL_007d;
			IL_007d:
			return result;
			IL_004a:
			result = true;
			goto IL_007d;
		}

		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.smeltable && !def.MadeFromStuff;
		}
	}
}
