using System;

namespace RimWorld
{
	// Token: 0x0200084C RID: 2124
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x06003034 RID: 12340 RVA: 0x001A3F7E File Offset: 0x001A237E
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06003035 RID: 12341 RVA: 0x001A3FA0 File Offset: 0x001A23A0
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfPlayer && !base.SelPawn.IsPrisoner;
			}
		}
	}
}
