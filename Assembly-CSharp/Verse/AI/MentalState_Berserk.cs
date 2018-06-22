using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A74 RID: 2676
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x06003B88 RID: 15240 RVA: 0x001F7B30 File Offset: 0x001F5F30
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x001F7B48 File Offset: 0x001F5F48
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x001F7B60 File Offset: 0x001F5F60
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
