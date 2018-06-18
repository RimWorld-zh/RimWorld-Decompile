using System;

namespace RimWorld
{
	// Token: 0x02000850 RID: 2128
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x0600303B RID: 12347 RVA: 0x001A3D9E File Offset: 0x001A219E
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x0600303C RID: 12348 RVA: 0x001A3DC0 File Offset: 0x001A21C0
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfPlayer && !base.SelPawn.IsPrisoner;
			}
		}
	}
}
