using System;

namespace RimWorld
{
	// Token: 0x02000851 RID: 2129
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		// Token: 0x0600303B RID: 12347 RVA: 0x001A3D33 File Offset: 0x001A2133
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x0600303C RID: 12348 RVA: 0x001A3D54 File Offset: 0x001A2154
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsPrisonerOfColony;
			}
		}
	}
}
