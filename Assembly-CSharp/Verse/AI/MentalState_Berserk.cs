using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A78 RID: 2680
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x06003B8B RID: 15243 RVA: 0x001F7748 File Offset: 0x001F5B48
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x001F7760 File Offset: 0x001F5B60
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x001F7778 File Offset: 0x001F5B78
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
