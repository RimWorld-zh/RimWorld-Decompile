using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7B RID: 2683
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x06003B9C RID: 15260 RVA: 0x001F7BE0 File Offset: 0x001F5FE0
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
