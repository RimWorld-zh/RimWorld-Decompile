using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000F6 RID: 246
	public class JoyGiver_WatchBuilding : JoyGiver_InteractBuilding
	{
		// Token: 0x0600052D RID: 1325 RVA: 0x00038F40 File Offset: 0x00037340
		protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			bool result;
			if (!base.CanInteractWith(pawn, t, inBed))
			{
				result = false;
			}
			else if (inBed)
			{
				Building_Bed bed = pawn.CurrentBed();
				result = WatchBuildingUtility.CanWatchFromBed(pawn, bed, t);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00038F88 File Offset: 0x00037388
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			IntVec3 c;
			Building t2;
			Job result;
			if (!WatchBuildingUtility.TryFindBestWatchCell(t, pawn, this.def.desireSit, out c, out t2))
			{
				result = null;
			}
			else
			{
				result = new Job(this.def.jobDef, t, c, t2);
			}
			return result;
		}
	}
}
