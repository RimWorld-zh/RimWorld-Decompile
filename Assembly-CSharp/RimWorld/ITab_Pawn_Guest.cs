using System;

namespace RimWorld
{
	// Token: 0x0200084E RID: 2126
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x06003037 RID: 12343 RVA: 0x001A4336 File Offset: 0x001A2736
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06003038 RID: 12344 RVA: 0x001A4358 File Offset: 0x001A2758
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfPlayer && !base.SelPawn.IsPrisoner;
			}
		}
	}
}
