using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A82 RID: 2690
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x06003BAF RID: 15279 RVA: 0x001F7DA8 File Offset: 0x001F61A8
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
