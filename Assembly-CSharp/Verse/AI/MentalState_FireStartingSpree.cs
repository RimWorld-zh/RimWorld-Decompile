using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A80 RID: 2688
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x06003BAE RID: 15278 RVA: 0x001F81E8 File Offset: 0x001F65E8
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
