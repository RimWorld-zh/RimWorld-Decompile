using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200052A RID: 1322
	public class DirectPawnRelation : IExposable
	{
		// Token: 0x04000E6A RID: 3690
		public PawnRelationDef def;

		// Token: 0x04000E6B RID: 3691
		public Pawn otherPawn;

		// Token: 0x04000E6C RID: 3692
		public int startTicks;

		// Token: 0x0600183B RID: 6203 RVA: 0x000D39FF File Offset: 0x000D1DFF
		public DirectPawnRelation()
		{
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x000D3A08 File Offset: 0x000D1E08
		public DirectPawnRelation(PawnRelationDef def, Pawn otherPawn, int startTicks)
		{
			this.def = def;
			this.otherPawn = otherPawn;
			this.startTicks = startTicks;
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x000D3A26 File Offset: 0x000D1E26
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
		}
	}
}
