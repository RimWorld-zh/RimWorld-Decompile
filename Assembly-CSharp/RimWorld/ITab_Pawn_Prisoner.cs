using System;

namespace RimWorld
{
	// Token: 0x0200084F RID: 2127
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		// Token: 0x0600303A RID: 12346 RVA: 0x001A412B File Offset: 0x001A252B
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x001A414C File Offset: 0x001A254C
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsPrisonerOfColony;
			}
		}
	}
}
