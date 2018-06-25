using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7E RID: 2686
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x06003BA9 RID: 15273 RVA: 0x001F84B4 File Offset: 0x001F68B4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
