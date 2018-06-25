using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A0 RID: 1696
	public interface ICommunicable
	{
		// Token: 0x0600241A RID: 9242
		string GetCallLabel();

		// Token: 0x0600241B RID: 9243
		string GetInfoText();

		// Token: 0x0600241C RID: 9244
		void TryOpenComms(Pawn negotiator);

		// Token: 0x0600241D RID: 9245
		Faction GetFaction();

		// Token: 0x0600241E RID: 9246
		FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator);
	}
}
