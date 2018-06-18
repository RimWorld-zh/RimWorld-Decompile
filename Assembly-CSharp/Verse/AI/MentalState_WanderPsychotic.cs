using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A81 RID: 2689
	public class MentalState_WanderPsychotic : MentalState
	{
		// Token: 0x06003BAD RID: 15277 RVA: 0x001F7D88 File Offset: 0x001F6188
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
