using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A77 RID: 2679
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x06003B97 RID: 15255 RVA: 0x001F7EF4 File Offset: 0x001F62F4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
