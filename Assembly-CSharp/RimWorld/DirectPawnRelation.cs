using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200052A RID: 1322
	public class DirectPawnRelation : IExposable
	{
		// Token: 0x04000E6E RID: 3694
		public PawnRelationDef def;

		// Token: 0x04000E6F RID: 3695
		public Pawn otherPawn;

		// Token: 0x04000E70 RID: 3696
		public int startTicks;

		// Token: 0x0600183A RID: 6202 RVA: 0x000D3C67 File Offset: 0x000D2067
		public DirectPawnRelation()
		{
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x000D3C70 File Offset: 0x000D2070
		public DirectPawnRelation(PawnRelationDef def, Pawn otherPawn, int startTicks)
		{
			this.def = def;
			this.otherPawn = otherPawn;
			this.startTicks = startTicks;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x000D3C8E File Offset: 0x000D208E
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
		}
	}
}
