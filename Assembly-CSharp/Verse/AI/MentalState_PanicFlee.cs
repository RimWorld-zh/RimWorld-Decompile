using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7B RID: 2683
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x06003B9A RID: 15258 RVA: 0x001F7B0C File Offset: 0x001F5F0C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
