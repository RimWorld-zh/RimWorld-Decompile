using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200015F RID: 351
	public abstract class WorkGiver_TakeToBed : WorkGiver_Scanner
	{
		// Token: 0x0600073F RID: 1855 RVA: 0x00048744 File Offset: 0x00046B44
		protected Building_Bed FindBed(Pawn pawn, Pawn patient)
		{
			return RestUtility.FindBedFor(patient, pawn, patient.HostFaction == pawn.Faction, false, false);
		}
	}
}
