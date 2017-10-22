using Verse;

namespace RimWorld
{
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			base.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			base.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		private Pawn BadHunter()
		{
			foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (item.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && WorkGiver_HunterHunt.HasShieldAndRangedWeapon(item))
				{
					return item;
				}
			}
			return null;
		}

		public override AlertReport GetReport()
		{
			Pawn pawn = this.BadHunter();
			return (pawn != null) ? AlertReport.CulpritIs((Thing)pawn) : false;
		}
	}
}
