using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A80 RID: 2688
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x06003BAB RID: 15275 RVA: 0x001F7D68 File Offset: 0x001F6168
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
