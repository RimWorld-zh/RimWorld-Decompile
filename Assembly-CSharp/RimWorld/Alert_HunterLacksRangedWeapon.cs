using Verse;

namespace RimWorld
{
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		public Alert_HunterLacksRangedWeapon()
		{
			base.defaultLabel = "HunterLacksWeapon".Translate();
			base.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			base.defaultPriority = AlertPriority.High;
		}

		public override AlertReport GetReport()
		{
			foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (item.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && !WorkGiver_HunterHunt.HasHuntingWeapon(item) && !item.Downed)
				{
					return item;
				}
			}
			return false;
		}
	}
}
