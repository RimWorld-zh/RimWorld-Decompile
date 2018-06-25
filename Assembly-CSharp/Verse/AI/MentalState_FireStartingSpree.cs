using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A81 RID: 2689
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x06003BAF RID: 15279 RVA: 0x001F8514 File Offset: 0x001F6914
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
