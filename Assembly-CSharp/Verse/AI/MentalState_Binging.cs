using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A78 RID: 2680
	public class MentalState_Binging : MentalState
	{
		// Token: 0x06003B91 RID: 15249 RVA: 0x001F7FD8 File Offset: 0x001F63D8
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
