using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7A RID: 2682
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x06003B9C RID: 15260 RVA: 0x001F834C File Offset: 0x001F674C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
