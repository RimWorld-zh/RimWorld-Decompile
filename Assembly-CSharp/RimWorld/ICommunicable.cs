using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200069E RID: 1694
	public interface ICommunicable
	{
		// Token: 0x06002416 RID: 9238
		string GetCallLabel();

		// Token: 0x06002417 RID: 9239
		string GetInfoText();

		// Token: 0x06002418 RID: 9240
		void TryOpenComms(Pawn negotiator);

		// Token: 0x06002419 RID: 9241
		Faction GetFaction();

		// Token: 0x0600241A RID: 9242
		FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator);
	}
}
