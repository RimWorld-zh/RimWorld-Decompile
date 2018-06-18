using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7F RID: 2687
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x06003BA9 RID: 15273 RVA: 0x001F7D48 File Offset: 0x001F6148
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
