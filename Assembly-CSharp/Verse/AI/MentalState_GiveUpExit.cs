using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7C RID: 2684
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x06003B9E RID: 15262 RVA: 0x001F7C00 File Offset: 0x001F6000
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
