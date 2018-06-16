using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A80 RID: 2688
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x06003BA9 RID: 15273 RVA: 0x001F7C94 File Offset: 0x001F6094
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
