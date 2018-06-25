using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7B RID: 2683
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x06003B9E RID: 15262 RVA: 0x001F836C File Offset: 0x001F676C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
