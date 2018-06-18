using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200052C RID: 1324
	public class DirectPawnRelation : IExposable
	{
		// Token: 0x06001840 RID: 6208 RVA: 0x000D38A7 File Offset: 0x000D1CA7
		public DirectPawnRelation()
		{
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x000D38B0 File Offset: 0x000D1CB0
		public DirectPawnRelation(PawnRelationDef def, Pawn otherPawn, int startTicks)
		{
			this.def = def;
			this.otherPawn = otherPawn;
			this.startTicks = startTicks;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x000D38CE File Offset: 0x000D1CCE
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
		}

		// Token: 0x04000E6D RID: 3693
		public PawnRelationDef def;

		// Token: 0x04000E6E RID: 3694
		public Pawn otherPawn;

		// Token: 0x04000E6F RID: 3695
		public int startTicks;
	}
}
