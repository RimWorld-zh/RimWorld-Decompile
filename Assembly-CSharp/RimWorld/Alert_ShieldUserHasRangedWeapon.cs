using System;
using Verse;

namespace RimWorld
{
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		public Alert_ShieldUserHasRangedWeapon()
		{
			base.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			base.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		public override AlertReport GetReport()
		{
			foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (item.equipment.Primary != null && item.equipment.Primary.def.IsRangedWeapon && item.apparel.WornApparel.Any((Predicate<Apparel>)((Apparel ap) => ap.def == ThingDefOf.Apparel_ShieldBelt)))
				{
					return (Thing)item;
				}
			}
			return false;
		}
	}
}
