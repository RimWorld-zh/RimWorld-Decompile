using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099A RID: 2458
	public class SpecialThingFilterWorker_NonSmeltableWeapons : SpecialThingFilterWorker
	{
		// Token: 0x06003733 RID: 14131 RVA: 0x001D87D8 File Offset: 0x001D6BD8
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.Smeltable;
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x001D8810 File Offset: 0x001D6C10
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

		// Token: 0x06003735 RID: 14133 RVA: 0x001D889C File Offset: 0x001D6C9C
		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.smeltable && !def.MadeFromStuff;
		}
	}
}
