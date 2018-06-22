using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A78 RID: 2680
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x06003B99 RID: 15257 RVA: 0x001F7F14 File Offset: 0x001F6314
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
