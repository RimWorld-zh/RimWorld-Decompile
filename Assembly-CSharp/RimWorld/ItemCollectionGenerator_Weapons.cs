using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_Weapons : ItemCollectionGenerator_Standard
	{
		private static List<ThingDef> weapons = new List<ThingDef>();

		public static void Reset()
		{
			ItemCollectionGenerator_Weapons.weapons.Clear();
			ItemCollectionGenerator_Weapons.weapons.AddRange(from x in ItemCollectionGeneratorUtility.allGeneratableItems
			where x.IsWeapon && !x.IsIngestible && x != ThingDefOf.WoodLog && x != ThingDefOf.ElephantTusk && (x.itemGeneratorTags == null || !x.itemGeneratorTags.Contains(ItemCollectionGeneratorUtility.SpecialRewardTag))
			select x);
		}

		protected override IEnumerable<ThingDef> AllowedDefs(ItemCollectionGeneratorParams parms)
		{
			TechLevel? techLevel2 = parms.techLevel;
			TechLevel techLevel = (!techLevel2.HasValue) ? TechLevel.Spacer : techLevel2.Value;
			if ((int)techLevel >= 5)
			{
				return from x in ItemCollectionGenerator_Weapons.weapons
				where (int)x.techLevel >= 4 && (int)x.techLevel <= (int)techLevel
				select x;
			}
			return from x in ItemCollectionGenerator_Weapons.weapons
			where (int)x.techLevel <= (int)techLevel
			select x;
		}
	}
}
