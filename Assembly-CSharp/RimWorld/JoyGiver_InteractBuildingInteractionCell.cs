using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000F4 RID: 244
	public class JoyGiver_InteractBuildingInteractionCell : JoyGiver_InteractBuilding
	{
		// Token: 0x06000527 RID: 1319 RVA: 0x00038DCC File Offset: 0x000371CC
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
