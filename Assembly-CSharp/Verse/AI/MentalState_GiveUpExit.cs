using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7C RID: 2684
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x06003B9C RID: 15260 RVA: 0x001F7B2C File Offset: 0x001F5F2C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
