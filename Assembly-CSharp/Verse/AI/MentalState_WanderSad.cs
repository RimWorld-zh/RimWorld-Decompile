using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7C RID: 2684
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x06003BA6 RID: 15270 RVA: 0x001F807C File Offset: 0x001F647C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
