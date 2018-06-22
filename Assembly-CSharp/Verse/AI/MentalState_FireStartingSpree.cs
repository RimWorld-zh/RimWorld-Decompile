using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7E RID: 2686
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x06003BAA RID: 15274 RVA: 0x001F80BC File Offset: 0x001F64BC
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
