using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A76 RID: 2678
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x06003B8C RID: 15244 RVA: 0x001F7C5C File Offset: 0x001F605C
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x001F7C74 File Offset: 0x001F6074
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x001F7C8C File Offset: 0x001F608C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
