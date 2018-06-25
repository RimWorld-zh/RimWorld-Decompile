using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7C RID: 2684
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x06003BA0 RID: 15264 RVA: 0x001F838A File Offset: 0x001F678A
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x001F83A0 File Offset: 0x001F67A0
		public override bool ForceHostileTo(Thing t)
		{
			return t.Faction != null && this.ForceHostileTo(t.Faction);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x001F83D0 File Offset: 0x001F67D0
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x001F83E8 File Offset: 0x001F67E8
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
