using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_InteractBuildingInteractionCell : JoyGiver_InteractBuilding
	{
		public JoyGiver_InteractBuildingInteractionCell()
		{
		}

		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			Job result;
			if (t.InteractionCell.Standable(t.Map) && !t.IsForbidden(pawn) && !t.InteractionCell.IsForbidden(pawn) && !pawn.Map.pawnDestinationReservationManager.IsReserved(t.InteractionCell))
			{
				result = new Job(this.def.jobDef, t, t.InteractionCell);
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
