using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7E RID: 2686
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x06003BAA RID: 15274 RVA: 0x001F81A8 File Offset: 0x001F65A8
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
