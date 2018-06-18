using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A78 RID: 2680
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x06003B8D RID: 15245 RVA: 0x001F781C File Offset: 0x001F5C1C
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x001F7834 File Offset: 0x001F5C34
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x001F784C File Offset: 0x001F5C4C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
