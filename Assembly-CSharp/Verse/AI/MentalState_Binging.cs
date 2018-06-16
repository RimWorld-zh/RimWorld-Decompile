using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A79 RID: 2681
	public class MentalState_Binging : MentalState
	{
		// Token: 0x06003B8F RID: 15247 RVA: 0x001F7798 File Offset: 0x001F5B98
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
