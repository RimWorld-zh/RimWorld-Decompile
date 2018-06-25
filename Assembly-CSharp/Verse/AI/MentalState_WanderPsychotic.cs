using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7F RID: 2687
	public class MentalState_WanderPsychotic : MentalState
	{
		// Token: 0x06003BAC RID: 15276 RVA: 0x001F81C8 File Offset: 0x001F65C8
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
