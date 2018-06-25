using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000CE RID: 206
	public class JobGiver_WanderInPartyArea : JobGiver_Wander
	{
		// Token: 0x060004AB RID: 1195 RVA: 0x00034F00 File Offset: 0x00033300
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 intVec;
			IntVec3 result;
			if (!PartyUtility.TryFindRandomCellInPartyArea(pawn, out intVec))
			{
				result = IntVec3.Invalid;
			}
			else
			{
				result = intVec;
			}
			return result;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00034F2E File Offset: 0x0003332E
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}
	}
}
