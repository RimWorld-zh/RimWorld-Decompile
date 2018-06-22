using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7B RID: 2683
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x06003BA4 RID: 15268 RVA: 0x001F805C File Offset: 0x001F645C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
