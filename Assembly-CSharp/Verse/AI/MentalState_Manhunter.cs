using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7D RID: 2685
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x06003B9E RID: 15262 RVA: 0x001F7B4A File Offset: 0x001F5F4A
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x001F7B60 File Offset: 0x001F5F60
		public override bool ForceHostileTo(Thing t)
		{
			return t.Faction != null && this.ForceHostileTo(t.Faction);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x001F7B90 File Offset: 0x001F5F90
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x001F7BA8 File Offset: 0x001F5FA8
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
