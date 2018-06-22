using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A75 RID: 2677
	public class MentalState_Binging : MentalState
	{
		// Token: 0x06003B8C RID: 15244 RVA: 0x001F7B80 File Offset: 0x001F5F80
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
