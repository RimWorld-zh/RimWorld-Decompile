using Verse;

namespace RimWorld
{
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		public Alert_BrawlerHasRangedWeapon()
		{
			base.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			base.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		public override AlertReport GetReport()
		{
			foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (item.story.traits.HasTrait(TraitDefOf.Brawler) && item.equipment.Primary != null && item.equipment.Primary.def.IsRangedWeapon)
				{
					return (Thing)item;
				}
			}
			return false;
		}
	}
}
