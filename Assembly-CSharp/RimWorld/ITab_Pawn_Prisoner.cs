using System;

namespace RimWorld
{
	// Token: 0x02000851 RID: 2129
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		// Token: 0x0600303D RID: 12349 RVA: 0x001A3DFB File Offset: 0x001A21FB
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x0600303E RID: 12350 RVA: 0x001A3E1C File Offset: 0x001A221C
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsPrisonerOfColony;
			}
		}
	}
}
