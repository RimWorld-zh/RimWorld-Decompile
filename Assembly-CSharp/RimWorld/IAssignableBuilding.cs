using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000679 RID: 1657
	public interface IAssignableBuilding
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060022EA RID: 8938
		IEnumerable<Pawn> AssigningCandidates { get; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060022EB RID: 8939
		IEnumerable<Pawn> AssignedPawns { get; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060022EC RID: 8940
		int MaxAssignedPawnsCount { get; }

		// Token: 0x060022ED RID: 8941
		void TryAssignPawn(Pawn pawn);

		// Token: 0x060022EE RID: 8942
		void TryUnassignPawn(Pawn pawn);

		// Token: 0x060022EF RID: 8943
		bool AssignedAnything(Pawn pawn);
	}
}
