using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A80 RID: 2688
	public class MentalState_WanderPsychotic : MentalState
	{
		// Token: 0x06003BAD RID: 15277 RVA: 0x001F84F4 File Offset: 0x001F68F4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
