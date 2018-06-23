using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000528 RID: 1320
	public class DirectPawnRelation : IExposable
	{
		// Token: 0x04000E6A RID: 3690
		public PawnRelationDef def;

		// Token: 0x04000E6B RID: 3691
		public Pawn otherPawn;

		// Token: 0x04000E6C RID: 3692
		public int startTicks;

		// Token: 0x06001837 RID: 6199 RVA: 0x000D38AF File Offset: 0x000D1CAF
		public DirectPawnRelation()
		{
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x000D38B8 File Offset: 0x000D1CB8
		public DirectPawnRelation(PawnRelationDef def, Pawn otherPawn, int startTicks)
		{
			this.def = def;
			this.otherPawn = otherPawn;
			this.startTicks = startTicks;
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x000D38D6 File Offset: 0x000D1CD6
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
		}
	}
}
