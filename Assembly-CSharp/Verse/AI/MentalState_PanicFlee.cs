using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A79 RID: 2681
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x06003B9B RID: 15259 RVA: 0x001F8020 File Offset: 0x001F6420
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
