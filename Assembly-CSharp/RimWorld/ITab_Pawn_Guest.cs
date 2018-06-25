using System;

namespace RimWorld
{
	// Token: 0x0200084E RID: 2126
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x06003038 RID: 12344 RVA: 0x001A40CE File Offset: 0x001A24CE
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x001A40F0 File Offset: 0x001A24F0
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfPlayer && !base.SelPawn.IsPrisoner;
			}
		}
	}
}
