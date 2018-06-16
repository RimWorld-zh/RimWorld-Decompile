using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7F RID: 2687
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x06003BA7 RID: 15271 RVA: 0x001F7C74 File Offset: 0x001F6074
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
