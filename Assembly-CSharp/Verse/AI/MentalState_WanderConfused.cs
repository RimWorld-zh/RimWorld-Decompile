using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7D RID: 2685
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x06003BA8 RID: 15272 RVA: 0x001F8188 File Offset: 0x001F6588
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
