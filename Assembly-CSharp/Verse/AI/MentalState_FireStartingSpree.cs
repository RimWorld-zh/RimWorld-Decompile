using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A82 RID: 2690
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x06003BAD RID: 15277 RVA: 0x001F7CD4 File Offset: 0x001F60D4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
