using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A77 RID: 2679
	public class MentalState_Binging : MentalState
	{
		// Token: 0x06003B90 RID: 15248 RVA: 0x001F7CAC File Offset: 0x001F60AC
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
