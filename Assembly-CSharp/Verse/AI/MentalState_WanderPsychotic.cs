using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A81 RID: 2689
	public class MentalState_WanderPsychotic : MentalState
	{
		// Token: 0x06003BAB RID: 15275 RVA: 0x001F7CB4 File Offset: 0x001F60B4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
