using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A77 RID: 2679
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x06003B8D RID: 15245 RVA: 0x001F7F88 File Offset: 0x001F6388
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x001F7FA0 File Offset: 0x001F63A0
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x001F7FB8 File Offset: 0x001F63B8
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
