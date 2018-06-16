using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A2 RID: 1698
	public interface ICommunicable
	{
		// Token: 0x0600241C RID: 9244
		string GetCallLabel();

		// Token: 0x0600241D RID: 9245
		string GetInfoText();

		// Token: 0x0600241E RID: 9246
		void TryOpenComms(Pawn negotiator);

		// Token: 0x0600241F RID: 9247
		Faction GetFaction();

		// Token: 0x06002420 RID: 9248
		FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator);
	}
}
