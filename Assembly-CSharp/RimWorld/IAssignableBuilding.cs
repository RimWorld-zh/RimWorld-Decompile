using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067B RID: 1659
	public interface IAssignableBuilding
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060022ED RID: 8941
		IEnumerable<Pawn> AssigningCandidates { get; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060022EE RID: 8942
		IEnumerable<Pawn> AssignedPawns { get; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060022EF RID: 8943
		int MaxAssignedPawnsCount { get; }

		// Token: 0x060022F0 RID: 8944
		void TryAssignPawn(Pawn pawn);

		// Token: 0x060022F1 RID: 8945
		void TryUnassignPawn(Pawn pawn);

		// Token: 0x060022F2 RID: 8946
		bool AssignedAnything(Pawn pawn);
	}
}
