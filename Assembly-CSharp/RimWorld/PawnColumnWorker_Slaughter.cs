using Verse;

namespace RimWorld
{
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Checkbox
	{
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		protected override bool GetValue(Pawn pawn)
		{
			return this.MarkedForSlaughter(pawn);
		}

		protected override void SetValue(Pawn pawn, bool value)
		{
			if (value != this.GetValue(pawn))
			{
				if (value)
				{
					pawn.MapHeld.designationManager.AddDesignation(new Designation((Thing)pawn, DesignationDefOf.Slaughter));
					SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
				}
				else
				{
					Designation slaughterDesignation = this.GetSlaughterDesignation(pawn);
					if (slaughterDesignation != null)
					{
						pawn.MapHeld.designationManager.RemoveDesignation(slaughterDesignation);
					}
				}
			}
		}

		private bool MarkedForSlaughter(Pawn pawn)
		{
			return this.GetSlaughterDesignation(pawn) != null;
		}

		private Designation GetSlaughterDesignation(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			return (mapHeld != null) ? mapHeld.designationManager.DesignationOn(pawn, DesignationDefOf.Slaughter) : null;
		}
	}
}
