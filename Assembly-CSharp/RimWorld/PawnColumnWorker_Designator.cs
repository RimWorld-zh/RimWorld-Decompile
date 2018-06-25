using System;
using Verse;

namespace RimWorld
{
	public abstract class PawnColumnWorker_Designator : PawnColumnWorker_Checkbox
	{
		protected PawnColumnWorker_Designator()
		{
		}

		protected abstract DesignationDef DesignationType { get; }

		protected virtual void Notify_DesignationAdded(Pawn pawn)
		{
		}

		protected override bool GetValue(Pawn pawn)
		{
			return this.GetDesignation(pawn) != null;
		}

		protected override void SetValue(Pawn pawn, bool value)
		{
			if (value != this.GetValue(pawn))
			{
				if (value)
				{
					pawn.MapHeld.designationManager.AddDesignation(new Designation(pawn, this.DesignationType));
					this.Notify_DesignationAdded(pawn);
				}
				else
				{
					Designation designation = this.GetDesignation(pawn);
					if (designation != null)
					{
						pawn.MapHeld.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		private Designation GetDesignation(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			Designation result;
			if (mapHeld == null)
			{
				result = null;
			}
			else
			{
				result = mapHeld.designationManager.DesignationOn(pawn, this.DesignationType);
			}
			return result;
		}
	}
}
