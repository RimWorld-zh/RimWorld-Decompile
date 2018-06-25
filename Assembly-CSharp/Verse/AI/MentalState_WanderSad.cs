using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7F RID: 2687
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x06003BAB RID: 15275 RVA: 0x001F84D4 File Offset: 0x001F68D4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
