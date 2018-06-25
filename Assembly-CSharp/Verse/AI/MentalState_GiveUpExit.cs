using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7A RID: 2682
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x06003B9D RID: 15261 RVA: 0x001F8040 File Offset: 0x001F6440
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
