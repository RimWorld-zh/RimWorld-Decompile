using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A2 RID: 1698
	public interface ICommunicable
	{
		// Token: 0x0600241E RID: 9246
		string GetCallLabel();

		// Token: 0x0600241F RID: 9247
		string GetInfoText();

		// Token: 0x06002420 RID: 9248
		void TryOpenComms(Pawn negotiator);

		// Token: 0x06002421 RID: 9249
		Faction GetFaction();

		// Token: 0x06002422 RID: 9250
		FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator);
	}
}
