using System;

namespace RimWorld
{
	// Token: 0x0200084D RID: 2125
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		// Token: 0x06003036 RID: 12342 RVA: 0x001A3FDB File Offset: 0x001A23DB
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06003037 RID: 12343 RVA: 0x001A3FFC File Offset: 0x001A23FC
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsPrisonerOfColony;
			}
		}
	}
}
