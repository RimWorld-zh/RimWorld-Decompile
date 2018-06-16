using System;

namespace RimWorld
{
	// Token: 0x02000850 RID: 2128
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x06003039 RID: 12345 RVA: 0x001A3CD6 File Offset: 0x001A20D6
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x0600303A RID: 12346 RVA: 0x001A3CF8 File Offset: 0x001A20F8
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfPlayer && !base.SelPawn.IsPrisoner;
			}
		}
	}
}
