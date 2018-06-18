using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A79 RID: 2681
	public class MentalState_Binging : MentalState
	{
		// Token: 0x06003B91 RID: 15249 RVA: 0x001F786C File Offset: 0x001F5C6C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
